package com.hw;

import android.app.Activity;
import android.content.Context;
import android.content.IntentSender;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Handler;
//import android.support.annotation.NonNull;
//import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.widget.Toast;


import com.adjust.sdk.Adjust;
import com.adjust.sdk.AdjustEvent;
import com.bytedance.applog.AppLog;
import com.gameanalytics.sdk.GameAnalytics;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.OnCanceledListener;
import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.Task;
import com.google.android.play.core.appupdate.AppUpdateInfo;
import com.google.android.play.core.appupdate.AppUpdateManager;
import com.google.android.play.core.appupdate.AppUpdateManagerFactory;
import com.google.android.play.core.install.InstallStateUpdatedListener;
import com.google.android.play.core.install.model.AppUpdateType;
import com.google.android.play.core.install.model.InstallStatus;
import com.google.android.play.core.install.model.UpdateAvailability;
import com.google.android.play.core.review.ReviewInfo;
import com.google.android.play.core.review.ReviewManager;
import com.google.android.play.core.review.ReviewManagerFactory;
import com.google.android.play.core.review.testing.FakeReviewManager;
import com.google.firebase.installations.FirebaseInstallations;
import com.google.firebase.installations.InstallationTokenResult;
//import com.google.firebase.remoteconfig.FirebaseRemoteConfig;
//import com.google.firebase.remoteconfig.FirebaseRemoteConfigSettings;

import com.hw.hwadssdk.HwAdsInterface;
import com.hw.hwadssdk.HwAdsInterstitialListener;
import com.hw.hwadssdk.HwAdsRewardVideoListener;
import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;

import com.google.firebase.analytics.FirebaseAnalytics;

import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Iterator;
//import java.util.ArrayList;
//import java.util.Iterator;
//import java.util.List;
//import java.util.Objects;
//import java.util.Set;


import androidx.annotation.NonNull;

import static android.Manifest.permission.ACCESS_COARSE_LOCATION;
import static android.Manifest.permission.WRITE_EXTERNAL_STORAGE;
public class GamePlayer {

    private  String TAG = "GamePlayer";
    public Activity mContext;
    public static final String SAMPLE_VERTICAL_CODE_ID = "945637904";
    private  int loadAderrorCount;
    private  int showAdCount;

    private  String AndroidPlatformWrapper = "AndroidPlatformWrapper";
    private  String rewardTag;
    private  String rewardActionSuccess;
    private  String rewardActionFail;
    private  String interActionSuccess;
    private  String interActionFail;
    private  String onLoadRewardResult = "OnLoadRewardResult";
    private  boolean isReward;

    private String gbid="264";
    private String apptoken="c9hwovbuifi8";
    private String bundleid="com.hg.cannonwar.android";
	private  boolean aTest = true;
    private ReviewManager manager;
    private ReviewInfo reviewInfo;
	private FirebaseAnalytics mFirebaseAnalytics;//firebase打点
	//初始化firebase
//	private FirebaseRemoteConfig mFirebaseRemoteConfig;

    private  AppUpdateManager m_appUpdateManager;
    public GamePlayer() {
    }

    public  void initHwSDK(Activity context, String url){
        mContext = context;
        Log.i(TAG, "start initHwSDK: "+ Thread.currentThread().getId());
        context.runOnUiThread(new Runnable() {
            @Override
            public void run() {
				Log.i(TAG, "after initHwSDK: "+ Thread.currentThread().getId());
                
                String ssid = AppLog.getSsid(); // 获取数说 ID
                Log.i(TAG, "ssid: "+ ssid);
				String newAb027 = AppLog.getAbConfig("AB027", "");
				Log.i(TAG, "Abtest:"+ newAb027);
				if (newAb027.equals("a")) {
				  aTest = true;
				} else if (newAb027.equals("b")) {
				  aTest = false;
				} else {
				  aTest = true;
				}
                Log.i(TAG, "isAbanben: "+ aTest);
                String abVersions = AppLog.getAbSdkVersion();
                Log.i("---abVersions---",""+abVersions);
                //doyourcode

				HwAdsInterface.initSDK(mContext,gbid,apptoken,bundleid,"yes");
                if (newAb027.equals("a")) {
				  HwAdsInterface.setLtvAbTest("a");
				} else if (newAb027.equals("b")) {
				  HwAdsInterface.setLtvAbTest("b");
				} else {
				  HwAdsInterface.setLtvAbTest("");
				}
				GameAnalytics.initializeWithGameKey(context,"1375ed0503b6291dbae2e8f33b61c3b3","cf1cf1d230165aae667b64527d36fc176bcb4441");
            }});


        setInterListerner();
        setRewardListener();
        manager =  ReviewManagerFactory.create(mContext);
        //manager = new FakeReviewManager(mContext); //模拟调用api是否正常，需要用上面代码发布测试渠道测试
        Task<ReviewInfo> request = manager.requestReviewFlow();
        request.addOnCompleteListener(task -> {
            if (task.isSuccessful()) {
                // We can get the ReviewInfo object
                reviewInfo = task.getResult();
                Log.i(TAG, "initHwSDK gp: "+ Thread.currentThread().getId());

            } else {
                // There was some problem, log or handle the error code.
                //@ReviewErrorCode int reviewErrorCode = ((TaskException) task.getException()).getErrorCode();
                Log.i(TAG, "initHwSDK gp error: ");
            }
        });

        //UpdateInApp();
    }
    ///应用内更新
    private  void  UpdateInApp(){
        m_appUpdateManager = AppUpdateManagerFactory.create(mContext);
        // Returns an intent object that you use to check for an update.
        Task<AppUpdateInfo> appUpdateInfoTask = m_appUpdateManager.getAppUpdateInfo();
        // Checks that the platform will allow the specified type of update.
        appUpdateInfoTask.addOnSuccessListener(appUpdateInfo -> {
            if (appUpdateInfo.updateAvailability() == UpdateAvailability.UPDATE_AVAILABLE
                    && appUpdateInfo.clientVersionStalenessDays() != null
                    && appUpdateInfo.clientVersionStalenessDays() >= 5
                    // This example applies an immediate update. To apply a flexible update
                    // instead, pass in AppUpdateType.FLEXIBLE
                    && appUpdateInfo.isUpdateTypeAllowed(AppUpdateType.FLEXIBLE)) {
                // Request the update.

                try {
                    //灵活更新
                    m_appUpdateManager.startUpdateFlowForResult(appUpdateInfo,AppUpdateType.FLEXIBLE,mContext,9001);
                } catch (IntentSender.SendIntentException e) {
                    e.printStackTrace();
                }
            }
            else if(appUpdateInfo.installStatus() == InstallStatus.DOWNLOADED){
                m_appUpdateManager.completeUpdate();//更新后并重启
//                try {
//                    m_appUpdateManager.startUpdateFlowForResult(appUpdateInfo,AppUpdateType.IMMEDIATE,mContext,9001);
//                } catch (IntentSender.SendIntentException e) {
//                    e.printStackTrace();
//                }
            }
        });

    }
    // Displays the snackbar notification and call to action.



    private void setRewardListener(){
        HwAdsInterface.setHwAdsRewardedVideoListener(new HwAdsRewardVideoListener() {
            @Override
            public void onRewardedVideoLoadSuccess() {
                //激励加载成功
                Log.i(TAG,"onRewardedVideoLoadSuccess");
                UnityPlayer.UnitySendMessage(AndroidPlatformWrapper, onLoadRewardResult, "true");
            }
            @Override
            public void onRewardedVideoLoadFailure() {
                //激励加载失败
                Log.i(TAG,"onRewardedVideoLoadFailure");
                UnityPlayer.UnitySendMessage(AndroidPlatformWrapper, onLoadRewardResult, "false");
            }
            @Override
            public void onRewardedVideoStarted() {
                //激励开始播放
                Log.e(TAG,"onRewardedVideoStarted");
            }
            @Override
            public void onRewardedVideoPlaybackError() {
                //激励播放失败
                Log.e(TAG,"onRewardedVideoPlaybackError");
                isReward = false;
            }
            @Override
            public void onRewardedVideoClicked() {
                //激励点击
                Log.e(TAG,"onRewardedVideoClicked");

            }
            @Override
            public void onRewardedVideoClosed() {
                //激励关闭
                Log.e(TAG,"onRewardedVideoClosed");
				if(rewardTag == null || rewardTag.isEmpty())
                    rewardTag = "";
                if (isReward)
                    UnityPlayer.UnitySendMessage(AndroidPlatformWrapper, rewardActionSuccess, rewardTag);
                else
                    UnityPlayer.UnitySendMessage(AndroidPlatformWrapper, rewardActionFail, rewardTag);
                isReward = false;
                UnityPlayer.UnitySendMessage(AndroidPlatformWrapper, onLoadRewardResult, "false");
            }
            @Override
            public void onRewardedVideoCompleted() {
                //激励完成观看
                //通常在这里做标记，在close的时候给奖励，动效，特效
                Log.e(TAG,"onRewardedVideoCompleted");
                isReward = true;
            }
        });
    }

    private void setInterListerner(){
        HwAdsInterface.setHwAdsInterstitialListener(new HwAdsInterstitialListener() {
            @Override
            public void onInterstitialLoaded() {
                //插屏加载成功
            }

            @Override
            public void onInterstitialFailed() {
                //插屏加载失败
            }

            @Override
            public void onInterstitialShown() {
                //插屏显示
            }

            @Override
            public void onInterstitialClicked() {
                //插屏点击
            }

            @Override
            public void onInterstitialDismissed(boolean b) {
                //插屏关闭
                Log.i(TAG, "onInterstitialDismissed: FacebookInter " +b);
                UnityPlayer.UnitySendMessage(AndroidPlatformWrapper, interActionSuccess, "");
            }
        });
    }


    public void showHwRewardAd(String arg1,String arg2,String arg3){
        mContext.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                rewardTag = arg1;
                rewardActionSuccess = arg2;
                rewardActionFail = arg3;
                Log.i(TAG,"showHwRewardAd: " + arg1 + "args2:" + arg2 + "args3:" + arg3);
                if(!HwAdsInterface.isRewardLoad()){
                    Log.e(TAG,"请先加载广告");
                    return;
                }
                isReward=false;
                HwAdsInterface.showReward(rewardTag);
            }}
            );



    }
    public void showHwInterAd(String arg1,String arg2,String arg3){
        Log.i(TAG, "showHwInterAd: ");
        mContext.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                interActionSuccess = arg2;
                interActionFail = arg3;
                if(!HwAdsInterface.isInterLoad()){
                    Log.e(TAG,"请先加载插屏广告");
                    UnityPlayer.UnitySendMessage(AndroidPlatformWrapper, interActionFail, "");
                    return;
                }
                HwAdsInterface.showInter();
            }}
        );
    }
    public boolean isHwRewardLoaded(){
        Log.i(TAG, "isHwRewardLoaded: " );
        if(!HwAdsInterface.isRewardLoad()){
            Log.e(TAG,"广告还没准备好，请先加载广告");

            return false;
        }
        return true;
    }

    public  boolean isHwInterLoaded(){
        Log.i(TAG, "isHwInterLoaded: " );
        if(!HwAdsInterface.isInterLoad()){
            Log.e(TAG,"插屏广告还没准备好，请先加载广告");
            return false;
        }
        return  true;
    }
    public  void  showHwBannerAd(){
        Log.i(TAG, "showHwBannerAd: ");
        mContext.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if(HwAdsInterface.isBannerLoad())
                    HwAdsInterface.showBanner();
            }}
        );
    }

    public void hideHwBannerAd(){
        Log.i(TAG, "hideHwBannerAd: ");
        mContext.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                HwAdsInterface.hideBanner();
            }}
        );
    }

    public  void  showComment(){
        Log.i(TAG, "showComment: ");
        if(manager != null && reviewInfo != null){
            Task<Void> flow = manager.launchReviewFlow(mContext, reviewInfo);
            flow.addOnCompleteListener(task -> {
                // The flow has finished. The API does not indicate whether the user
                // reviewed or not, or even whether the review dialog was shown. Thus, no
                // matter the result, we continue our app flow.
                Log.i(TAG, "showComment: success");
            });
        }
    }

    public  boolean HwABTest()
    {
        Log.i(TAG, "aTest:"+aTest);
        return aTest;
    }



    public void OnApplicationPause(boolean pause) {
        Log.i(TAG, "OnApplicationPause: " + pause);
    }


    public  void  TAEventPropertie(String custom,String dic){
        Log.i(TAG,"\"TAEventPropertie:  custom:"+custom+"=>dic:"+dic);
        try{
            JSONObject paramsObj = new JSONObject(dic);
            Log.i(TAG, "TAEventPropertie paramsObj:" + paramsObj);
            AppLog.onEventV3(custom,paramsObj);
            if(paramsObj.length() > 0){
                Iterator iter = paramsObj.keys();
                while (iter.hasNext()) {
                    String gaEventKey = (String) iter.next();
                    String gaEventValue = paramsObj.getString(gaEventKey);
                    if(custom != null && gaEventKey != null && gaEventValue != null){
                        String gaEvent = custom+":"+gaEventKey+":"+gaEventValue;
                        if(gaEvent != null){
                            Log.i(TAG,"GameAnalytics gaEvent:"+gaEvent);
                            GameAnalytics.addDesignEventWithEventId(gaEvent);

                        }
                    }

                }
            }
        }
        catch (JSONException e)
        {
            Log.i(TAG,"TAEventPropertie: " + e.toString());
            e.printStackTrace();
        }
    }
	
	public  void  HwAdjustEvent(String token,boolean isSetOrderId)
    {
        Log.i(TAG, "HwAdjustEvent token:" + token );
        //HwAdsInterface.HwAnalyticsUserNew(token,"","","");
        AdjustEvent adjustEvent = new AdjustEvent(token);
        if (isSetOrderId)
        {
            adjustEvent.setOrderId(token);
        }
        Adjust.trackEvent(adjustEvent);
    }


    public void HwAnalyticsPurchaseSecondVerify(String number,String token,String productId,int purchaseType,String orderId)
    {
        Log.i(TAG, "HwAnalyticsPurchaseSecondVerify productId:" + productId );
        HwAdsInterface.HwAnalyticsPurchaseSecondVerify("HwPurchase",number,token,productId,purchaseType,orderId,"");
    }

	public void HwAboutFirebase(String category,String action,String label)
	{
		Log.i(TAG, "aboutFirebase:");
		if(mFirebaseAnalytics==null)
		{
			mFirebaseAnalytics = FirebaseAnalytics.getInstance(mContext);
		}
    // [START custom_event]
		Bundle params = new Bundle();
		//params.putString(action, label);
		mFirebaseAnalytics.logEvent(category, params);
    // [END custom_event]
	}
}
