//----------------------------------------------
//            ColaFramework
// Copyright © 2018-2049 ColaFramework 马三小伙儿
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ColaFramework.Foundation;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ColaFramework.ToolKit
{
    /// <summary>
    /// ColaFramework 编辑器助手类
    /// </summary>
    public class ColaEditHelper
    {
        public static string overloadedDevelopmentServerURL = "";
        public static string m_projectRoot;
        public static string m_projectRootWithSplit;

        /// <summary>
        /// 编辑器会用到的一些临时目录
        /// </summary>
        public static string TempCachePath
        {
            get { return Path.Combine(Application.dataPath, "../ColaCache"); }
        }

        public static string ProjectRoot
        {
            get
            {
                if (string.IsNullOrEmpty(m_projectRoot))
                {
                    m_projectRoot = FileHelper.FormatPath(Path.GetDirectoryName(Application.dataPath));
                }

                return m_projectRoot;
            }
        }

        public static string ProjectRootWithSplit
        {
            get
            {
                if (string.IsNullOrEmpty(m_projectRootWithSplit))
                {
                    m_projectRootWithSplit = ProjectRoot + "/";
                }

                return m_projectRootWithSplit;
            }
        }

        /// <summary>
        /// 打开指定文件夹(编辑器模式下)
        /// </summary>
        /// <param name="path"></param>
        public static void OpenDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            path = path.Replace("/", "\\");
            if (!Directory.Exists(path))
            {
                Debug.LogError("No Directory: " + path);
                return;
            }

            if (!path.StartsWith("file://"))
            {
                path = "file://" + path;
            }

            Application.OpenURL(path);
        }

        public static T GetScriptableObjectAsset<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
            }

            return asset;
        }

        public static void CreateOrReplacePrefab(GameObject gameobject, string path,
            ReplacePrefabOptions options = ReplacePrefabOptions.ConnectToPrefab)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            if (prefab != null)
            {
                PrefabUtility.ReplacePrefab(gameobject, prefab, options);
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                PrefabUtility.CreatePrefab(path, gameobject, options);
            }
        }

        #region 打包相关方法实现

        

        public static string GetPlatformName()
        {
            return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
        }

        private static string GetPlatformForAssetBundles(BuildTarget target)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "iOS";
                case BuildTarget.WebGL:
                    return "WebGL";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";
#if UNITY_2017_3_OR_NEWER
                case BuildTarget.StandaloneOSX:
                    return "OSX";
#else
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSX:
                    return "OSX";
#endif
                default:
                    return null;
            }
        }


        private static Dictionary<string, string> GetVersions(AssetBundleManifest manifest)
        {
            var items = manifest.GetAllAssetBundles();
            return items.ToDictionary(item => item, item => manifest.GetAssetBundleHash(item).ToString());
        }

        private static Dictionary<string, ABFileInfo> LoadVersions(string versionsTxt)
        {
            if (!File.Exists(versionsTxt))
                return null;
            return FileHelper.ReadABVersionInfo(versionsTxt);
        }

        private static void SaveVersions(string versionsTxt, string path, Dictionary<string, string> versions)
        {
            if (File.Exists(versionsTxt))
                File.Delete(versionsTxt);
            var sb = new StringBuilder(64);
            foreach (var item in versions)
            {
                var fileSize = FileHelper.GetFileSizeKB(path + "/" + item.Key);
                sb.Append(string.Format("{0}:{1}:{2}:{3}\n", item.Key, item.Value, fileSize, fileSize));
            }

            FileHelper.WriteString(versionsTxt, sb.ToString());
        }

        public static void RemoveUnusedAssetBundleNames()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }

        public static void SetAssetBundleNameAndVariant(string assetPath, string bundleName, string variant)
        {
            var importer = AssetImporter.GetAtPath(assetPath);
            if (importer == null) return;
            importer.assetBundleName = bundleName;
            if (!string.IsNullOrEmpty(variant))
            {
                importer.assetBundleVariant = variant;
            }
        }

        

        public static string TrimedAssetDirName(string assetDirName)
        {
            assetDirName = assetDirName.Replace("\\", "/");
            assetDirName = assetDirName.Replace(Constants.GameAssetBasePath, "");
            return Path.GetDirectoryName(assetDirName).Replace("\\", "/");
            ;
        }

        


        public static string GetServerURL()
        {
            string downloadURL;
            if (string.IsNullOrEmpty(overloadedDevelopmentServerURL) == false)
            {
                downloadURL = overloadedDevelopmentServerURL;
            }
            else
            {
                IPHostEntry host;
                string localIP = "";
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }

                downloadURL = "http://" + localIP + ":7888/";
            }

            return downloadURL;
        }

        #region 处理Lua代码

        

        public static void EncodeLuaFile(string srcFile, string outFile)
        {
            //if (!srcFile.ToLower().EndsWith(".lua"))
            //{
            //    File.Copy(srcFile, outFile, true);
            //    return;
            //}
            //bool isWin = true;
            //string luaexe = string.Empty;
            //string args = string.Empty;
            //string exedir = string.Empty;
            //string currDir = Directory.GetCurrentDirectory();
            //if (Application.platform == RuntimePlatform.WindowsEditor)
            //{
            //    isWin = true;
            //    luaexe = "luajit.exe";
            //    args = "-b -g " + srcFile + " " + outFile;
            //    exedir = AppDataPath.Replace("assets", "") + "LuaEncoder/luajit/";
            //}
            //else if (Application.platform == RuntimePlatform.OSXEditor)
            //{
            //    isWin = false;
            //    luaexe = "./luajit";
            //    args = "-b -g " + srcFile + " " + outFile;
            //    exedir = AppDataPath.Replace("assets", "") + "LuaEncoder/luajit_mac/";
            //}
            //Directory.SetCurrentDirectory(exedir);
            //ProcessStartInfo info = new ProcessStartInfo();
            //info.FileName = luaexe;
            //info.Arguments = args;
            //info.WindowStyle = ProcessWindowStyle.Hidden;
            //info.UseShellExecute = isWin;
            //info.ErrorDialog = true;
            //Util.Log(info.FileName + " " + info.Arguments);

            //Process pro = Process.Start(info);
            //pro.WaitForExit();
            //Directory.SetCurrentDirectory(currDir);
        }

        #endregion

        /// <summary>
        /// 清除所有的AB Name
        /// </summary>
        public static void ClearAllAssetBundleName()
        {
            string[] oldAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            var length = oldAssetBundleNames.Length;
            for (int i = 0; i < length; i++)
            {
                EditorUtility.DisplayProgressBar("清除AssetBundleName", "正在清除AssetBundleName", i * 1f / length);
                AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[i], true);
            }

            EditorUtility.ClearProgressBar();
        }

        public static string TrimedAssetBundleName(string assetBundleName)
        {
            return assetBundleName.Replace(Constants.GameAssetBasePath, "");
        }

        /// <summary>
        /// 标记一个文件夹下所有文件为一个BundleName
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bundleName"></param>
        public static void MarkAssetsToOneBundle(string path, string bundleName)
        {
            if (Directory.Exists(path))
            {
                bundleName = bundleName.ToLower();
                var files = FileHelper.GetAllChildFiles(path);
                var length = files.Length;
                for (int i = 0; i < length; i++)
                {
                    var fileName = files[i];
                    if (fileName.EndsWith(".meta", StringComparison.Ordinal) ||
                        fileName.EndsWith(".cs", System.StringComparison.CurrentCulture))
                    {
                        continue;
                    }

                    EditorUtility.DisplayProgressBar("玩命处理中", string.Format("正在标记第{0}个文件... {1}/{2}", i, i, length),
                        i * 1.0f / length);
                    var assetPath = files[i].Replace(ProjectRootWithSplit, "");
                    SetAssetBundleNameAndVariant(assetPath, bundleName, null);
                }

                EditorUtility.ClearProgressBar();
            }
        }

        /// <summary>
        /// 按文件夹进行标记AssetBundleName
        /// </summary>
        /// <param name="path"></param>
        public static void MarkAssetsWithDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.LogError("MarkAssetsWithDir Error! Path: " + path + "is not exist!");
                return;
            }

            var files = FileHelper.GetAllChildFiles(path);
            var length = files.Length;
            for (var i = 0; i < files.Length; i++)
            {
                var fileName = files[i];
                if (fileName.EndsWith(".meta", StringComparison.Ordinal) ||
                    fileName.EndsWith(".cs", System.StringComparison.CurrentCulture))
                {
                    continue;
                }

                EditorUtility.DisplayProgressBar("玩命处理中", string.Format("正在标记第{0}个文件... {1}/{2}", i, i, length),
                    i * 1.0f / length);
                var assetBundleName = TrimedAssetBundleName(Path.GetDirectoryName(fileName).Replace("\\", "/")) + "_g" +
                                      AppConst.ExtName;
                var assetPath = fileName.Replace(ProjectRootWithSplit, "");
                SetAssetBundleNameAndVariant(assetPath, assetBundleName.ToLower(), null);
            }

            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 按文件进行标记AssetBundleName
        /// </summary>
        /// <param name="path"></param>
        public static void MarkAssetsWithFile(string path)
        {
            if (File.Exists(path))
            {
                if (path.EndsWith(".meta", StringComparison.Ordinal) ||
                    path.EndsWith(".cs", System.StringComparison.CurrentCulture))
                {
                    return;
                }

                var dir = Path.GetDirectoryName(path);
                var name = Path.GetFileNameWithoutExtension(path);
                if (dir == null)
                    return;
                if (name == null)
                    return;
                dir = dir.Replace("\\", "/") + "/";
                var assetBundleName = TrimedAssetBundleName(Path.Combine(dir, name)) + AppConst.ExtName;
                SetAssetBundleNameAndVariant(path, assetBundleName.ToLower(), null);
            }
            else if (Directory.Exists(path))
            {
                var files = FileHelper.GetAllChildFiles(path);
                var length = files.Length;
                for (var i = 0; i < files.Length; i++)
                {
                    var fileName = files[i];
                    if (fileName.EndsWith(".meta", StringComparison.Ordinal) ||
                        fileName.EndsWith(".cs", System.StringComparison.CurrentCulture))
                    {
                        continue;
                    }

                    EditorUtility.DisplayProgressBar("玩命处理中", string.Format("正在标记第{0}个文件... {1}/{2}", i, i, length),
                        i * 1.0f / length);

                    var dir = Path.GetDirectoryName(path);
                    var name = Path.GetFileNameWithoutExtension(path);
                    if (dir == null)
                        return;
                    if (name == null)
                        return;
                    dir = dir.Replace("\\", "/") + "/";
                    var assetBundleName = TrimedAssetBundleName(Path.Combine(dir, name)) + AppConst.ExtName;
                    var assetPath = fileName.Replace(ProjectRootWithSplit, "");
                    SetAssetBundleNameAndVariant(assetPath, assetBundleName.ToLower(), null);
                }
            }
            else
            {
                Debug.LogError("MarkAssetsWithFile Error! Path: " + path + "is not exist!");
                return;
            }

            EditorUtility.ClearProgressBar();
        }

        #endregion
    }
}