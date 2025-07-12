using System;
using GamePush;

public static class AddManager 
{
    public static void ShowInterstitial(Action CallBack = null) {
        // Show Interstitial
        GP_Ads.ShowFullscreen(CallBack);

        if (CallBack != null)
            CallBack.Invoke();
    }

    public static void ShowReward(Action<bool> CallBack) {
        // Show Reward
        GP_Ads.ShowRewarded();
        CallBack.Invoke(true);
    }
}

