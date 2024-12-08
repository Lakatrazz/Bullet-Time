using BoneLib;
using BoneLib.BoneMenu;

using MelonLoader;

using Il2CppSLZ.Marrow;

using UnityEngine;

namespace BulletTime;

public class BulletTimeMod : MelonMod
{
    public const string Version = "1.1.0";

    public static bool IsEnabled { get; private set; }
    public static int MaxSlowMo { get; private set; }
    public static bool SpeedTime { get; private set; }

    public static MelonPreferences_Category MelonPrefCategory { get; private set; }
    public static MelonPreferences_Entry<bool> MelonPrefEnabled { get; private set; }
    public static MelonPreferences_Entry<int> MelonPrefMaxSlowMo { get; private set; }
    public static MelonPreferences_Entry<bool> MelonPrefSpeedTime { get; private set; }


    private static bool _preferencesSetup = false;

    public static Page MainPage { get; private set; }
    public static BoolElement BoneMenuEnabledElement { get; private set; }
    public static IntElement BoneMenuMaxSlowMoElement { get; private set; }
    public static BoolElement BoneMenuSpeedTimeElement { get; private set; }

    public override void OnInitializeMelon()
    {
        Hooking.OnLevelLoaded += OnLevelLoaded;

        SetupMelonPrefs();
        SetupBoneMenu();
    }

    public static void SetupMelonPrefs()
    {
        MelonPrefCategory = MelonPreferences.CreateCategory("BulletTime");
        MelonPrefEnabled = MelonPrefCategory.CreateEntry("IsEnabled", true);
        MelonPrefMaxSlowMo = MelonPrefCategory.CreateEntry("MaxSlowMo", 6);
        MelonPrefSpeedTime = MelonPrefCategory.CreateEntry("SpeedTime", false);

        IsEnabled = MelonPrefEnabled.Value;
        MaxSlowMo = MelonPrefMaxSlowMo.Value;
        SpeedTime = MelonPrefSpeedTime.Value;

        _preferencesSetup = true;
    }

    public override void OnPreferencesLoaded()
    {
        if (!_preferencesSetup)
        {
            return;
        }

        IsEnabled = MelonPrefEnabled.Value;
        BoneMenuEnabledElement.Value = IsEnabled;

        MaxSlowMo = MelonPrefMaxSlowMo.Value;
        BoneMenuMaxSlowMoElement.Value = MaxSlowMo;

        SpeedTime = MelonPrefSpeedTime.Value;
        BoneMenuSpeedTimeElement.Value = SpeedTime;

        OnValueChanged();
    }

    public static void OnLevelLoaded(LevelInfo info)
    {
        OnValueChanged();
    }

    public static void SetupBoneMenu()
    {
        MainPage = Page.Root.CreatePage("Bullet Time", Color.green);
        BoneMenuEnabledElement = MainPage.CreateBool("Mod Toggle", Color.yellow, IsEnabled, OnSetEnabled);
        BoneMenuMaxSlowMoElement = MainPage.CreateInt("Max Slow Mo", Color.yellow, MaxSlowMo, 1, 0, 32, OnSetMax);
        BoneMenuSpeedTimeElement = MainPage.CreateBool("Speed Time", Color.yellow, SpeedTime, OnSetSpeedTime);
    }

    public static void OnSetEnabled(bool value)
    {
        IsEnabled = value;
        MelonPrefEnabled.Value = value;
        MelonPrefCategory.SaveToFile(false);
        OnValueChanged();
    }

    public static void OnSetSpeedTime(bool value)
    {
        SpeedTime = value;
        MelonPrefSpeedTime.Value = value;
        MelonPrefCategory.SaveToFile(false);
        OnValueChanged();
    }

    public static void OnSetMax(int value)
    {
        MaxSlowMo = value;
        MelonPrefMaxSlowMo.Value = value;
        MelonPrefCategory.SaveToFile(false);
        OnValueChanged();
    }

    public static void OnValueChanged()
    {
        if (IsEnabled)
        {
            TimeManager.max_intensity = Mathf.Pow(2, MaxSlowMo);
            TimeManager.max_timeScaleStep = MaxSlowMo;
        }
        else
        {
            TimeManager.max_intensity = 8f;
            TimeManager.max_timeScaleStep = 3;
        }
    }
}
