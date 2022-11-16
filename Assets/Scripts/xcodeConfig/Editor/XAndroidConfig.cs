using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using System.IO;
using UnityEngine;
using System;

public class XAndroidConfig : MonoBehaviour
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.Android)
        {
            string buglytoolPath =  Path.Combine(System.Environment.CurrentDirectory, "Buglytools");
            PostProcessAndroidBuild(buglytoolPath);
        }
    }
    public static void PostProcessAndroidBuild(string pathToBuiltProject)
    {
        ScriptingImplementation backend =
            PlayerSettings.GetScriptingBackend(UnityEditor.BuildTargetGroup
                .Android); //as UnityEditor.ScriptingImplementation;

        if (backend == UnityEditor.ScriptingImplementation.IL2CPP)
        {
            CopyAndroidIL2CPPSymbols(pathToBuiltProject, PlayerSettings.Android.targetArchitectures);
        }
    }

    public static void CopyAndroidIL2CPPSymbols(string pathToBuiltProject, AndroidArchitecture targetDevice)
    {
        //string buildName = Path.GetFileNameWithoutExtension(pathToBuiltProject);
        //FileInfo fileInfo = new FileInfo(pathToBuiltProject);
        //string symbolsDir = fileInfo.Directory.Name;
        //symbolsDir = symbolsDir + "/" + buildName + "_IL2CPPSymbols";
        string symbolsDir = pathToBuiltProject;
        CreateDir(symbolsDir);
        //CopyARMSymbols(symbolsDir);
        //CopyX86Symbols(symbolsDir);
        //CopyARM64ymbols(symbolsDir);



        //switch (PlayerSettings.Android.targetArchitectures)
        //{
        //    case AndroidArchitecture.All:
        //        {
        //            CopyARMSymbols(symbolsDir);
        //            CopyX86Symbols(symbolsDir);
        //            CopyARM64ymbols(symbolsDir);
        //            break;
        //        }
        //    case AndroidArchitecture.ARMv7:
        //        {
        //            CopyARMSymbols(symbolsDir);
        //            break;
        //        }
        //    case AndroidArchitecture.X86:
        //        {
        //            CopyX86Symbols(symbolsDir);
        //            break;
        //        }

        //    default:
        //        break;
        //}
    }


    const string libpath = "/../Temp/StagingArea/symbols/";
    const string libFilename = "libil2cpp.so.debug";

    private static void CopyARMSymbols(string symbolsDir)
    {
        string sourcefileARM = Path.GetFullPath(Application.dataPath + libpath + "armeabi-v7a/" + libFilename);
        CreateDir(symbolsDir + "/armeabi-v7a/");
        try
        {
            File.Copy(sourcefileARM, symbolsDir + "/armeabi-v7a/libil2cpp.so.debug");
            Debug.Log("sourcefileARM: " + sourcefileARM);
        }
        catch (Exception e)
        {
            Debug.LogError("CopyARMSymbolsError: " + e.Message);
        }
    }

    private static void CopyX86Symbols(string symbolsDir)
    {
        string sourcefileX86 = Path.GetFullPath(Application.dataPath + libpath + "x86/" + libFilename);
        try
        {
            File.Copy(sourcefileX86, symbolsDir + "/x86/libil2cpp.so.debug");
            Debug.Log("sourcefileX86: " + sourcefileX86);
        }
        catch (Exception e)
        {
            Debug.LogError("CopyX86Symbols: " + e.Message);
        }
    }

    private static void CopyARM64ymbols(string symbolsDir)
    {
        string sourcefileARM64 = Path.GetFullPath(Application.dataPath + libpath + "arm64-v8a/" + libFilename);
        CreateDir(symbolsDir + "/arm64-v8a/");
        try
        {
            File.Copy(sourcefileARM64, symbolsDir + "/arm64-v8a/libil2cpp.so.debug"); 
            Debug.Log("sourcefileARM64: " + sourcefileARM64);
        }
        catch (Exception e)
        {
            Debug.LogError("CopyARM64ymbols: " + e.Message);
        }
    }

    public static void CreateDir(string path)
    {
        if (Directory.Exists(path))
            return;

        Directory.CreateDirectory(path);
    }
}


 





