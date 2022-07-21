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
        [MenuItem("HybridCLR/����Dll/ActiveBuildTarget")]
        static void CopeByActive()
        {
            Copy(EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("HybridCLR/����Dll/StandaloneWindows64")]
        static void CopeByStandaloneWindows64()
        {
            Copy(BuildTarget.StandaloneWindows64);
        }
        [MenuItem("HybridCLR/����Dll/Android")]
        static void CopeByAndroid()
        {
            Copy(BuildTarget.Android);
        }
        [MenuItem("HybridCLR/����Dll/IOS")]
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
                    Debug.LogError($"ab�����AOT����Ԫ����dll:{dllPath} ʱ��������,�ļ������ڡ���Ҫ����һ��������������ɲü����AOT dll");
                    continue;
                }
                string dllBytesPath = $"{exportDir}/{dll}.bytes";
                File.Copy(dllPath, dllBytesPath, true);
            }
            AssetDatabase.Refresh();
            Debug.Log("�ȸ�Dll���Ƴɹ���");
        }
    }
}


