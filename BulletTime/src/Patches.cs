using HarmonyLib;

using Il2CppSLZ.Marrow;

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

[HarmonyPatch(typeof(OpenControllerRig))]
public static class OpenControllerRigPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(OpenControllerRig.TimeInput))]
    public static bool TimeInputPrefix(OpenControllerRig __instance, bool down, bool up, bool touch)
    {
        if (!BulletTimeMod.IsEnabled)
        {
            return true;
        }

        if (!down)
        {
            return true;
        }

        if (TimeManager.CurrentTimeScaleStep >= 3)
        {
            TimeManager.DECREASE_TIMESCALE();

            __instance._timeInput = false;
            return false;
        }

        if (TimeManager.CurrentTimeScaleStep >= TimeManager.max_timeScaleStep)
        {
            __instance._timeInput = false;

            return false;
        }

        return true;
    }
}