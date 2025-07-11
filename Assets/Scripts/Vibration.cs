using UnityEngine;

public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    private static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    private static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    private static bool isMobilePlatform = Application.isMobilePlatform;
#endif

    public static void Vibrate(long milliseconds = 250)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        vibrator.Call("vibrate", milliseconds);
#elif UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Handheld.Vibrate();
        }
#elif UNITY_EDITOR
        // No vibration in editor
#endif
    }

    public static void VibratePeek()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        long[] pattern = { 0, 50 };
        vibrator.Call("vibrate", pattern, -1);
#elif UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Handheld.Vibrate();
        }
#elif UNITY_EDITOR
        // No vibration in editor
#endif
    }

    public static void VibratePop()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        long[] pattern = { 0, 30, 50, 30 };
        vibrator.Call("vibrate", pattern, -1);
#elif UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Handheld.Vibrate();
        }
#elif UNITY_EDITOR
        // No vibration in editor
#endif
    }
}
