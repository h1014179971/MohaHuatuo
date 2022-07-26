using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFramework
{
    public static class UIVariable
    {
        public static readonly Vector2 ScreenCenterPos = new Vector2((UIController.UIRoot as RectTransform).sizeDelta.x / 2, (UIController.UIRoot as RectTransform).sizeDelta.y / 2);
    }
    
    public static class UIName
    {
        public static readonly string MainCamera = "MainCamera";
        public static readonly string UICanvas = "UICanvas";   
        public static readonly string UIContent = "UIContent";
        public static readonly string UIBlocker = "UITopPage";
        public static readonly string UIMainPage = "UIMainPage";
        public static readonly string UIStagePage = "UIStagePage";
        public static readonly string UIStageWinWindow = "UIStageWinWindow";
    }

    public static class UIPrefabPath
    {
        private static readonly string UI_ROOT = "UI/";
        private static readonly string UI_Common = "Common/";
        private static readonly string UI_Top = "Top/";
        private static readonly string UI_Riddle = "Riddle/";
        

        public static readonly string UI_CANVAS = UI_ROOT  + UIName.UICanvas;
        public static readonly string UI_Blocker = "Game/Prefabs/UI/Blocker/UIBlocker"; //UI_ROOT + UI_Top + UIName.UIBlocker;
        public static readonly string UI_MainPage = "Game/Prefabs/UI/MainPage/UIMainPage";
        public static readonly string UI_TopPage = "Game/Prefabs/UI/TopPage/UITopPage";
        public static readonly string UI_HandBookWindow = "Game/Prefabs/UI/HandBookWindow/UIHandBookWindow";
        public static readonly string UI_GamePropsWindow = "Game/Prefabs/UI/GamePropsWindow/UIGamePropsWindow";
        public static readonly string UI_CurrentResultWindow = "Game/Prefabs/UI/CurrentResultWindow/UICurrentResultWindow";
        public static readonly string UI_StagePage = UI_ROOT + UI_Riddle + UIName.UIStagePage;
        public static readonly string UI_StageWinWindow = UI_ROOT + UI_Riddle + UIName.UIStageWinWindow;

        public static readonly string UI_Idle = UI_ROOT + "Idle";


        public static readonly string UI_PmWindow = "Game/Prefabs/UI/PMWindow/UIPmWindow";

    }
    public static class SceneName
    {
        public static readonly string SCENE_GAME = "game_";
        public static readonly string SCENE_CHAPTER = "chapter";
        public static readonly string SCENE_LEVEL = "_";
    }



    public static class UIColor
    {
        public static Color32 WHITE = new Color32(255, 255, 255, 255);
        public static Color32 BLACK = new Color32(0, 0, 0, 255);
        public static Color32 RED = new Color32(255, 0, 0, 255);
        public static Color32 GREEN = new Color32(0, 255, 0, 255);
    }

    public static class MaterialPath
    {
        public static readonly string Mat_UIBlueMask = "Game/Materials/greyblue";
    }


}