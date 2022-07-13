﻿//----------------------------------------------
//            ColaFramework
// Copyright © 2018-2049 ColaFramework 马三小伙儿
//----------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace libx
{
    /// <summary>
    /// 资源加载的对外接口，封装平台和细节，可对Lua导出
    /// </summary>
    public static class AssetLoader
    {
        private const int CHECK_INTERVAL = 10;
        private static float time = 0f;
        private static Dictionary<string, WeakReference> AssetReferences = new Dictionary<string, WeakReference>(32);
        private static Dictionary<string, AssetRequest> LoadedAssets = new Dictionary<string, AssetRequest>(32);
        private static List<string> UnUsedAssets = new List<string>(16);
        private static bool hasAB = true;

        /// <summary>
        /// 根据类型和路径返回相应的资源(同步方法)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Load<T>(string path) where T : Object
        {
            if (hasAB)
                return Load(path, typeof(T)) as T;
            else
                return Resources.Load<T>(path);
        }

        /// <summary>
        /// 根据类型和路径返回相应的资源(同步方法)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Object Load(string path, Type type)
        {
            if (hasAB)
                return LoadInternal(path, type);
            else
                return Resources.Load(path,type);
            //return LoadInternal(path, type);
        }

        private static Object LoadInternal(string path, Type type)
        {
            WeakReference wkRef = null;
            if (AssetReferences.TryGetValue(path, out wkRef))
            {
                if (CheckAssetAlive(wkRef.Target))
                {
                    return wkRef.Target as Object;
                }
            }
            AssetRequest assetProxy = Assets.LoadAsset(path, type);
            var asset = assetProxy.asset;
            wkRef = new WeakReference(asset);
            AssetRequest assetRef = null;
            if (LoadedAssets.TryGetValue(path, out assetRef))
            {
                LoadedAssets[path] = assetProxy;
                Assets.UnloadAsset(assetRef);
            }
            else
            {
                LoadedAssets.Add(path, assetProxy);
            }
            AssetReferences[path] = wkRef;
            return asset;
        }


        /// <summary>
        /// 根据类型和路径返回相应的资源(异步方法)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public static void LoadAsync<T>(string path, Action<Object> callback) where T : Object
        {
            LoadAsync(path, typeof(T), callback);
        }

        /// <summary>
        /// 根据类型和路径返回相应的资源(异步方法)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="t"></param>
        public static void LoadAsync(string path, Type type, Action<Object> callback)
        {
            LoadAsyncInternal(path, type,callback);
        }

        public static void LoadAsyncInternal(string path, Type type, Action<Object> callback)
        {
            WeakReference wkRef = null;
            if (AssetReferences.TryGetValue(path, out wkRef))
            {
                if (CheckAssetAlive(wkRef.Target))
                {
                    callback(wkRef.Target as Object);
                }
            }
            var assetProxy = Assets.LoadAssetAsync(path, type);
            assetProxy.completed += (obj) =>
            {
                wkRef = new WeakReference(obj.asset);
                //wkRef.Target = obj.asset;
                var asset = obj.asset;
                AssetRequest assetRef = null;
                if (LoadedAssets.TryGetValue(path, out assetRef))
                {
                    LoadedAssets[path] = assetProxy;
                    Assets.UnloadAsset(assetRef);
                }
                else
                {
                    LoadedAssets.Add(path, assetProxy);
                }
                AssetReferences[path] = wkRef;
                callback(asset);
            };
        }

        private static bool CheckAssetAlive(System.Object asset)
        {
            if (null == asset) { return false; }
            if (asset is Object)
            {
                Object UnityObject = asset as Object;
                if (null == UnityObject || !UnityObject)
                {
                    return false;
                }
            }
            else
            {
                throw new Exception(string.Format("InVaild Asset Type:{0}", asset.GetType()));
            }
            return true;
        }

        public static void Update(float deltaTime)
        {
            time += deltaTime;
            if (time < CHECK_INTERVAL) { return; }
            time = 0;
            foreach (KeyValuePair<string, WeakReference> kvPair in AssetReferences)
            {
                if (false == CheckAssetAlive(kvPair.Value.Target))
                {
                    UnUsedAssets.Add(kvPair.Key);
                }
            }
            if (UnUsedAssets.Count > 0)
            {
                foreach (var name in UnUsedAssets)
                {
                    Debug.Log("卸载无用资源:" + name);
                    AssetReferences.Remove(name);
                    AssetRequest asset = null;
                    if (LoadedAssets.TryGetValue(name, out asset))
                    {
                        Assets.UnloadAsset(asset);
                        LoadedAssets.Remove(name);
                    }
                }
                UnUsedAssets.Clear();
            }
        }

        public static void Release()
        {
            time = 0;
            //强制卸载所有的资源

        }
    }

}