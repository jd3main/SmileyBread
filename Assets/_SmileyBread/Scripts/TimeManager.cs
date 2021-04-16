using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeManager
{
    private static int freezeCount = 0;
    public static float prevTimeScale = 0;

    public static void Freeze(int n=1)
    {
        if (freezeCount == 0)
        {
            prevTimeScale = Time.timeScale;
        }
        Time.timeScale = 0;
        freezeCount += n;
    }

    public static void Unfreeze(int n=1)
    {
        if (freezeCount < n)
        {
            throw new System.Exception($"try to unfreeze {n} times but only {freezeCount} freezes available");
        }

        freezeCount -= n;
        if (freezeCount == 0)
        {
            Time.timeScale = prevTimeScale;
        }
    }
}
