package com.hw;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Handler;
//import android.support.annotation.NonNull;
//import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.widget.Toast;



import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;
import java.util.Objects;
import java.util.Set;



import static android.Manifest.permission.ACCESS_COARSE_LOCATION;
import static android.Manifest.permission.WRITE_EXTERNAL_STORAGE;
public class GamePlayer {

    private  String TAG = "GamePlayer";
    public Activity mContext;
    public static final String SAMPLE_VERTICAL_CODE_ID = "946192870";
    private  int loadAderrorCount;

    private  String AndroidPlatformWrapper = "AndroidPlatformWrapper";
    private  String rewardTag;
    private  String rewardActionSuccess;
    private  String rewardActionFail;
    private  String interActionSuccess;
    private  String interActionFail;
    private  String onLoadRewardResult = "OnLoadRewardResult";
    private  boolean isReward;
    // 是否正在加载广告
    private boolean isLoadingAd;
    public GamePlayer() {
    }

    public  void initHwSDK(Activity context, String url){
        mContext = context;
        Log.i(TAG, "initHwSDK: "+ Thread.currentThread().getId());


       



    }
    /**
     * 加载广告
     *
     * @param codeId      广告ID
     * @param orientation 广告展示方向
     */
    private void loadAd(String codeId, int orientation) {

    }


    


    public void showHwRewardAd(String arg1,String arg2,String arg3){
        Log.i(TAG, "showHwRewardAd: "+arg2);
        mContext.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                rewardActionSuccess = arg2;
                rewardActionFail = arg3;
                UnityPlayer.UnitySendMessage(AndroidPlatformWrapper,"PlatformCallback_FinishRewardAd","true");
            }});

    }

    public void showHwInterAd(String arg1,String arg2,String arg3) {
        Log.i(TAG, "showHwInterAd: ");
        mContext.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                interActionSuccess = arg2;
                interActionFail = arg3;
                UnityPlayer.UnitySendMessage(AndroidPlatformWrapper, interActionSuccess, "");
            }});
    }
    public boolean isHwRewardLoaded(){
        Log.i(TAG, "isHwRewardLoaded: " );
        return true;
    }

    public boolean isHwInterLoaded() {
        Log.i(TAG, "isHwInterLoaded: ");
        return true;
    }
    public  void  showHwBannerAd(){
        Log.i(TAG, "showHwBannerAd: ");
    }

    public void hideHwBannerAd(){
        Log.i(TAG, "hideHwBannerAd: ");
    }

    public  void  showComment(){
        Log.i(TAG, "showComment: ");

    }
	public  boolean HwABTest()
    {
        return true;
    }


    public void OnApplicationPause(boolean pause) {
        Log.i(TAG, "OnApplicationPause: " + pause);
    }


    public void TAEventPropertie(String custom, String dic) {
        Log.i(TAG, "TAEventPropertie custom:" + custom + "=>dic:" + dic);
    }
	public  void  HwAdjustEvent(String token,boolean isSetOrderId)
    {
        Log.i(TAG, "HwAdjustEvent token:" + token );
    }


    public void HwAnalyticsPurchaseSecondVerify(String number,String token,String productId,int purchaseType,String orderId)
    {
        Log.i(TAG, "HwAnalyticsPurchaseSecondVerify productId:" + productId );
    }



}
