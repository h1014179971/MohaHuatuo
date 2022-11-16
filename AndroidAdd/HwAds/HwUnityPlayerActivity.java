package com.hw;
import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.Window;

import com.adjust.sdk.Adjust;
import com.unity3d.player.*;

public class HwUnityPlayerActivity extends UnityPlayerActivity {
    @Override
    protected void onCreate(Bundle bundle) {
        super.onCreate(bundle);
    }
    // Pause Unity
    @Override protected void onPause()
    {
        super.onPause();
        Adjust.onPause();
    }

    // Resume Unity
    @Override protected void onResume()
    {
        super.onResume();
        Adjust.onResume();

    }
}
