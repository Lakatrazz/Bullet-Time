using HarmonyLib;

using UnityEngine;

namespace BulletTime.Patching;

[HarmonyPatch(typeof(Time))]
public static class TimePatches
{
    [HarmonyPatch(nameof(Time.timeScale))]
    [HarmonyPatch(MethodType.Setter)]
    [HarmonyPrefix]
    public static void SetTimescale(ref float value)
    {
        // Make sure speed time is enabled
        if (!BulletTimeMod.IsEnabled || !BulletTimeMod.SpeedTime)
        {
            return;
        }

        // Inverse timescale
        if (value < 1f && value > 0f)
        {
            value = 1f / value;
        }
    }
}
