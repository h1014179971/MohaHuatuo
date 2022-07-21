using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using HybridCLR;

namespace Huatuo
{
    public class CopeDll2Assets 
    {
        [MenuItem("HybridCLR/复制Dll/ActiveBuildTarget")]
        static void CopeByActive()
        {
            Copy(EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("HybridCLR/复制Dll/StandaloneWindows64")]
        static void CopeByStandaloneWindows64()
        {
            Copy(BuildTarget.StandaloneWindows64);
        }
        [MenuItem("HybridCLR/复制Dll/Android")]
        static void CopeByAndroid()
        {
            Copy(BuildTarget.Android);
        }
        [MenuItem("HybridCLR/复制Dll/IOS")]
        static void CopeByIOS()
        {
            Copy(BuildTarget.iOS);
        }

        static void Copy(BuildTarget target)
        {
            string exportDir = Path.Combine(Application.dataPath, "GameAssets/Dlls"); 
            if (!Directory.Exists(exportDir))
            {
                Directory.CreateDirectory(exportDir);
            }
            string aotDllDir = $"{BuildConfig.AssembliesPostIl2CppStripDir}/{target}";
            foreach (var dll in LoadDll.aotDlls)
            {
                string dllPath = $"{aotDllDir}/{dll}";
                if (!File.Exists(dllPath))
                {
                    Debug.LogError($"ab中添加AOT补充元数据dll:{dllPath} 时发生错误,文件不存在。需要构建一次主包后才能生成裁剪后的AOT dll");
                    continue;
                }
                string dllBytesPath = $"{exportDir}/{dll}.bytes";
                File.Copy(dllPath, dllBytesPath, true);
            }
            AssetDatabase.Refresh();
            Debug.Log("热更Dll复制成功！");
        }
    }
}


