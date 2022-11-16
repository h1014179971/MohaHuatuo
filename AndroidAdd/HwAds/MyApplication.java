package com.hw;

import android.Manifest;
import android.app.Activity;
import android.app.Application;
import android.os.Bundle;
//import android.support.multidex.MultiDex;
import android.util.Log;

import androidx.core.app.ActivityCompat;

import com.adjust.sdk.Adjust;
import com.adjust.sdk.AdjustAttribution;
import com.adjust.sdk.AdjustConfig;
import com.adjust.sdk.AdjustEventFailure;
import com.adjust.sdk.AdjustEventSuccess;
import com.adjust.sdk.LogLevel;
import com.adjust.sdk.OnAttributionChangedListener;
import com.adjust.sdk.OnEventTrackingFailedListener;
import com.adjust.sdk.OnEventTrackingSucceededListener;
import com.bytedance.applog.InitConfig;
import com.bytedance.applog.util.UriConstants;
import com.tencent.bugly.*;
import com.tencent.bugly.crashreport.CrashReport;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import com.bytedance.applog.AppLog;

public class MyApplication extends Application {
    private static final String TAG = "MyApplication";
    @Override
    public void onCreate() {
        super.onCreate();
        Log.i(TAG, "初始化: " );
        //MultiDex.install(this);
        /*bugly*/
        String buglyId = "a1b37ed2f9";
        Log.i(TAG, "buglyId:"+buglyId);
        CrashReport.initCrashReport(getApplicationContext(), buglyId, false);
        /*bugly*/

        /*dataplayer*/
        //初始化
        final InitConfig config = new InitConfig("333965","Google Play");
        Log.i(TAG, "Init dataplayer");
        //数据上报
        config.setUriConfig(UriConstants.SINGAPORE_ALI);
        config.setAbEnable(true);

        config.setLogger ((msg, t) -> Log.d (TAG, msg, t)); // 是否在控制台输出日志，可用于观察用户行为日志上报情况，建议仅在调试时使用
        config.setEnablePlay(true); // 是否开启游戏模式，游戏APP建议设置为 true

        // 加密开关，SDK 5.5.1 及以上版本支持，false 为关闭加密，上线前建议设置为 true
        AppLog.setEncryptAndCompress(true);

        config.setAutoStart(true);
        AppLog.init(this, config);
        /* 初始化结束 */
        /*dataplayer*/
    }

}