using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace NeverFall
{
    public sealed class CollectionEffectSystem : MonoBehaviour
    {
        public bool PlayOnAwake
        {
            get => m_PlayOnAwake;
            set => m_PlayOnAwake = value;
        }

        public RectTransform Destination
        {
            get => BaseSprite == null ? GetComponent<RectTransform>() : m_Destination;
            set => m_Destination = value;
        }

        public float ExplosionTime
        {
            get => m_ExplosionTime;
            set => m_ExplosionTime = value;
        }

        public float MoveTime
        {
            get => m_MoveTime;
            set => m_MoveTime = value;
        }

        public float DiffusionRange
        {
            get => m_DiffusionRange;
            set => m_DiffusionRange = value;
        }

        public Sprite BaseSprite
        {
            get => m_BaseSprite;
            set => m_BaseSprite = value;
        }

        public int ParticlesCount
        {
            get => m_ParticlesCount;
            set => m_ParticlesCount = value;
        }

        public float ParticlesSize
        {
            get => m_ParticlesSize;
            set => m_ParticlesSize = value;
        }

        public float ExplosionQueuingTime
        {
            get => m_ExplosionQueuingTime;
            set => m_ExplosionQueuingTime = value;
        }

        public bool IsTargetResponse
        {
            get => m_IsTargetResponse;
            set => m_IsTargetResponse = value;
        }

        public float MoveQueuingTime
        {
            get => m_MoveQueuingTime;
            set => m_MoveQueuingTime = value;
        }

        [Header("[是否开启测试模式]")]
        [SerializeField]
        private bool m_TestMode;

        [Header("[设置特效属性]")]
        [Tooltip("粒子目的地（每个粒子飞向的目标位置）")]
        [SerializeField]
        private RectTransform m_Destination;

        [Tooltip("基本精灵（粒子的图像）")]
        [SerializeField]
        private Sprite m_BaseSprite;

        [Tooltip("唤醒时播放（在该效果激活时立即播放动画）")]
        [SerializeField]
        private bool m_PlayOnAwake;

        [Tooltip("爆发时间（爆开阶段的时间）")]
        [SerializeField]
        [Range(0, 10)]
        private float m_ExplosionTime;

        [Tooltip("移动时间（移动阶段的时间）")]
        [SerializeField]
        [Range(0, 10)]
        private float m_MoveTime;

        [Tooltip("爆发范围（爆开的范围）")]
        [SerializeField]
        [Range(0, 100)]
        private float m_DiffusionRange;

        [Tooltip("粒子数量")]
        [SerializeField]
        [Range(0, 50)]
        private int m_ParticlesCount;

        [Tooltip("粒子大小")]
        [SerializeField]
        [Range(0, 10)]
        private float m_ParticlesSize;

        [Tooltip("爆发排队时间（按此时间依次生成粒子）")]
        [SerializeField]
        [Range(0, 20)]
        private float m_ExplosionQueuingTime;

        [Tooltip("移动排队时间（按此时间依次移动粒子）")]
        [SerializeField]
        [Range(0, 20)]
        private float m_MoveQueuingTime;

        [Tooltip("目标是否响应（粒子的目标是否播放缩放动画）")]
        [SerializeField]
        private bool m_IsTargetResponse;

        private UnityEvent m_OnPerParticleComplete = new UnityEvent();

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            for (var i = 0; i < ParticlesCount; i++)
            {
                var go = CreateChild(i);
                go.SetActive(false);
            }

            if (m_PlayOnAwake)
            {
                MoveTo(Destination);
            }
        }

        public void Play(Transform parent, Vector3 startP, UnityAction callBack = null)
        {
            if (parent != null)
            {
                transform.SetParent(parent);
                transform.position = startP;
                transform.localScale = Vector3.one;
            }

            m_OnPerParticleComplete.RemoveAllListeners();
            m_OnPerParticleComplete.AddListener(callBack);
            Reset();
            Init();
            MoveTo(Destination);
        }

        /// <summary>
        /// 所有子对象移动到目的地
        /// </summary>
        /// <param name="pos">目的地</param>
        /// <param name="OnPerParticleArrive">到达目的的回调</param>
        public void MoveTo(RectTransform pos, UnityEvent OnPerParticleArrive = null)
        {
            var i = 0;
            Observable.Interval(TimeSpan.FromSeconds(ExplosionQueuingTime)).TimeInterval()
                .Take(ParticlesCount)
                .Subscribe(a =>
                {
                    var child = GetChild(i);
                    child.gameObject.SetActive(true);
                    child.DOMove(GetRandomPoint(), ExplosionTime);
                    Observable.Interval(TimeSpan.FromSeconds(ExplosionTime + MoveQueuingTime * i)).TimeInterval()
                        .Take(1)
                        .Subscribe(b =>
                        {
                            if (pos != null)
                            {
                                child.DOMove(pos.position, MoveTime).OnComplete(() =>
                                {
                                    OnPerParticleArrive?.Invoke();
                                    if (pos != null)
                                    {
                                        if (IsTargetResponse)
                                        {
                                            pos.DOScale(Vector3.one * 1.2f, 0.2f).ChangeStartValue(pos.localScale)
                                                .OnComplete(() =>
                                                {
                                                    if (pos != null)
                                                    {
                                                        pos.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                                                        {
                                                            if (child == GetChild(0))
                                                            {
                                                                m_OnPerParticleComplete?.Invoke();
                                                            }

                                                            if (child == GetChild(ParticlesCount - 1))
                                                            {
                                                                SG.ResourceManager.Instance.ReturnObjectToPool(gameObject);
                                                            }
                                                        });
                                                    }
                                                    else
                                                    {
                                                        SG.ResourceManager.Instance.ReturnObjectToPool(gameObject);
                                                    }
                                                });
                                        }
                                    }
                                    else
                                    {
                                        SG.ResourceManager.Instance.ReturnObjectToPool(gameObject);
                                    }
                                    child.gameObject.SetActive(false);
                                    child.localPosition = Vector3.zero;
                                });
                            }
                            else
                            {
                                SG.ResourceManager.Instance.ReturnObjectToPool(gameObject);
                            }

                        });
                    i++;
                });
        }

        /// <summary>
        /// 创建子对象（在初始化时创建）
        /// </summary>
        /// <param name="nameIndex">赋予索引</param>
        private GameObject CreateChild(int nameIndex)
        {
            var go = GetChild(nameIndex)?.gameObject;
            if (go == null)
            {
                go = new GameObject($"Sub_{nameIndex}");
            }
            go.transform.SetParent(transform);
            go.SetActive(true);
            var image = go.GetOrAddComponent<Image>();
            image.sprite = BaseSprite;
            image.preserveAspect = true;
            image.SetNativeSize();
            var rect = go.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.one * 0.5f;
            rect.anchorMin = Vector2.one * 0.5f;
            rect.anchorMax = Vector2.one * 0.5f;
            rect.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;
            rect.sizeDelta *= ParticlesSize;
            return go;
        }

        /// <summary>
        /// 获取圆内随机的点
        /// </summary>
        private Vector3 GetRandomPoint()
        {
            var pos = Random.insideUnitSphere * DiffusionRange + transform.position;
            return new Vector3(pos.x, pos.y, transform.position.z);
        }

        /// <summary>
        /// 通过索引来查找子对象
        /// </summary>
        /// <param name="index">索引</param>
        private RectTransform GetChild(int index)
        {
            if (transform.childCount > index)
            {
                return transform?.GetChild(index).GetComponent<RectTransform>();

            }
            return null;
        }

        /// <summary>
        /// 重置所有子对象的位置并隐藏
        /// </summary>
        private void Reset()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = GetChild(i);
                child.localPosition = Vector3.zero;
                child.gameObject.SetActive(false);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, DiffusionRange);
        }
    }
}
