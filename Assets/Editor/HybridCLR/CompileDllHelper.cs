﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Player;
using UnityEngine;

namespace HybridCLR
{
    internal class CompileDllHelper
    {
        public static void CompileDll(string buildDir, BuildTarget target)
        {
            var group = BuildPipeline.GetBuildTargetGroup(target);

            ScriptCompilationSettings scriptCompilationSettings = new ScriptCompilationSettings();
            scriptCompilationSettings.group = group;
            scriptCompilationSettings.target = target;
            Directory.CreateDirectory(buildDir);
            ScriptCompilationResult scriptCompilationResult = PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, buildDir);
            foreach (var ass in scriptCompilationResult.assemblies)
            {
                Debug.LogFormat("compile assemblies:{1}/{0}", ass, buildDir);
            }
            if (target == EditorUserBuildSettings.activeBuildTarget)
            {
                var hotfixDlls = new List<string>()
                {
                    "GameFix.dll",
                };
                string exportDir = Path.Combine(Application.dataPath, "GameAssets/Dlls");
                foreach (var dll in hotfixDlls)
                {
                    string dllPath = $"{BuildConfig.GetHotFixDllsOutputDirByTarget(target)}/{dll}";
                    string dllBytesPath = $"{exportDir}/{dll}.bytes";
                    File.Copy(dllPath, dllBytesPath, true);
                }
                AssetDatabase.Refresh();
                Debug.Log("热更Dll复制成功！");
            }
        }

        public static void CompileDll(BuildTarget target)
        {
            CompileDll(BuildConfig.GetHotFixDllsOutputDirByTarget(target), target);
        }

        [MenuItem("HybridCLR/CompileDll/ActiveBuildTarget")]
        public static void CompileDllActiveBuildTarget()
        {
            CompileDll(EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("HybridCLR/CompileDll/Win32")]
        public static void CompileDllWin32()
        {
            CompileDll(BuildTarget.StandaloneWindows);
        }

        [MenuItem("HybridCLR/CompileDll/Win64")]
        public static void CompileDllWin64()
        {
            CompileDll(BuildTarget.StandaloneWindows64);
        }

        [MenuItem("HybridCLR/CompileDll/Android")]
        public static void CompileDllAndroid()
        {
            CompileDll(BuildTarget.Android);
        }

        [MenuItem("HybridCLR/CompileDll/IOS")]
        public static void CompileDllIOS()
        {
            CompileDll(BuildTarget.iOS);
        }
    }
}
