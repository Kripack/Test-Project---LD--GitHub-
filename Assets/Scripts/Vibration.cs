using UnityEngine;
using System.Collections;

public static class Vibration
{

    public enum VibroType { Short, Default, Long }

    public static float _shortTime = 0.01f;
    public static float _defaultTime = 0.15f;
    public static float _longTime = 0.3f;

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    public static void Vibrate(VibroType _type)
    {
        switch (_type)
        {
            case VibroType.Short:
                {
                    Vibrate(_shortTime);
                    break;
                }
            case VibroType.Default:
                {
                    Vibrate(_defaultTime);
                    break;
                }
            case VibroType.Long:
                {
                    Vibrate(_longTime);
                    break;
                }
        }
    }

    private static void Vibrate(float time)
    {
        long timeLong = long.Parse((Mathf.RoundToInt(time * 1000)).ToString("0"));
        Vibration.Vibrate(timeLong);
    }

    public static void Vibrate()
    {
        if (!GeneralSettings.Instance.GetVibro())
            return;
        if (isAndroid())
            vibrator.Call("vibrate");
        else
            Handheld.Vibrate();
    }

    public static void Vibrate(long milliseconds)
    {
        if (!GeneralSettings.Instance.GetVibro())
            return;
        if (isAndroid())
            vibrator.Call("vibrate", milliseconds);
        else
            Handheld.Vibrate();
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (!GeneralSettings.Instance.GetVibro())
            return;
        if (isAndroid())
            vibrator.Call("vibrate", pattern, repeat);
        else
            Handheld.Vibrate();
    }

    public static bool HasVibrator()
    {
        return isAndroid();
    }

    public static void Cancel()
    {
        if (isAndroid())
            vibrator.Call("cancel");
    }

    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
        return false;
#endif
    }
}