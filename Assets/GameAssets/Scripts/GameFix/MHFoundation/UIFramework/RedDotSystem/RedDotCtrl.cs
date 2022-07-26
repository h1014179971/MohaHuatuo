using Foundation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFramework
{
    public partial class RedDotCtrl : Singleton<RedDotCtrl>
    {
        /// <summary>
        /// 红点数据;
        /// </summary>
        Dictionary<RedDotType, RedDotBase> RedDotConditionDict = new Dictionary<RedDotType, RedDotBase>();
        /// <summary>
        /// 红点物体;
        /// </summary>
        Dictionary<RedDotType, List<RedDotItem>> RedDotObjDict = new Dictionary<RedDotType, List<RedDotItem>>();
        /// <summary>
        /// 初始化红点系统(注意只需要初始化一次);
        /// </summary>
        //public override void Init()
        //{
        //    RedDotConditionDict.Clear();

        //    // 添加红点数据判断;
        //    RedDotConditionDict.Add(RedDotType.Email_UnReadSystem, new RedDot_EmailUnReadSystem());
        //    RedDotConditionDict.Add(RedDotType.Email_UnReadPlayer, new RedDot_EmailUnReadPlayer());
        //}

        /// <summary>
        /// 注册红点;
        /// </summary>
        /// <param name="redDotType"></param>
        /// <param name="item"></param>
        public void RegisterRedDot(List<RedDotType> redDotTypes, RedDotItem item)
        {
            for (int i = 0; i < redDotTypes.Count; i++)
            {
                RegisterRedDot(redDotTypes[i], item);
            }
        }
        /// <summary>
        /// 取消注册红点;
        /// </summary>
        /// <param name="item"></param>
        public void UnRegisterRedDot(RedDotItem item)
        {
            var itor = RedDotObjDict.GetEnumerator();
            while (itor.MoveNext())
            {
                List<RedDotItem> redDotItems = itor.Current.Value;
                if (redDotItems.Contains(item))
                {
                    redDotItems.Remove(item);
                    break;
                }
            }
        }

        /// <summary>
        /// 检查红点提示;
        /// </summary>
        /// <param name="redDotType"></param>
        /// <returns></returns>
        public bool Check(List<RedDotType> redDotTypes, object[] objs = null, object[] selfObjs = null)
        {
            for (int i = 0; i < redDotTypes.Count; i++)
            {
                //只要有一个需要点亮,就显示;
                if (Check(redDotTypes[i], objs, selfObjs))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 更新该类型的所有红点组件;
        /// </summary>
        /// <param name="redDotType"></param>
        /// <returns></returns>
        public void NotifyAll(RedDotType redDotType, object[] objs = null)
        {
            if (RedDotObjDict.ContainsKey(redDotType))
            {
                for (int i = 0; i < RedDotObjDict[redDotType].Count; i++)
                {
                    RedDotItem item = RedDotObjDict[redDotType][i];
                    if (item != null)
                    {
                        List<RedDotType> redDotTypes = item.GetRedDotTypes();
                        bool bCheck = Check(redDotTypes, objs, item.selfDatas);
                        item.SetData(bCheck);
                    }
                }
            }
        }
        #region private
        /// <summary>
        /// 添加红点界面;
        /// </summary>
        /// <param name="redDotType"></param>
        /// <param name="item"></param>
        private void RegisterRedDot(RedDotType redDotType, RedDotItem item)
        {
            if (RedDotObjDict.ContainsKey(redDotType))
            {
                RedDotObjDict[redDotType].Add(item);
            }
            else
            {
                List<RedDotItem> items = new List<RedDotItem>();
                items.Add(item);
                RedDotObjDict.Add(redDotType, items);
            }
        }
        /// <summary>
        /// 检查红点提示,内部调用;
        /// </summary>
        /// <param name="redDotType"></param>
        /// <returns></returns>
        private bool Check(RedDotType redDotType, object[] objs, object[] selfObjs)
        {
            try
            {
                if (RedDotConditionDict.ContainsKey(redDotType))
                {
                    return RedDotConditionDict[redDotType].ShowRedDot(objs, selfObjs);
                }
            }
            catch (Exception err)
            {
                LogUtility.LogError(err);
            }

            return false;
        }
        #endregion
    }
}


