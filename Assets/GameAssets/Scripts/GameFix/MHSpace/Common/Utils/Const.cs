using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHSpace
{
    public static class FixScreen
    {
        public static readonly float width = 1080;
        public static readonly float height = 1920;
        public static readonly float unit = 0.01f;//一个像素大小是0.01个unity单位
        public static readonly float idleCameraSize = 9.6f;
    }
    public static class Files
    {
        public static readonly string jsonFolder = "JsonData";
        public static readonly string player = "Player";
        public static readonly string map = "Map.json";
        public static readonly string ballAuto = "BallAuto.json";
        public static readonly string unitConvert = "UnitConvert.json";
        public static readonly string powerInfo = "PowerInfo.json";
    }
    public static class Tags
    {
        public static readonly string map = "Map";
    }

    public static class UIPrefab
    {
        public static readonly string UI_GamePage = "UIGamePage.prefab";
    }

}

