using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.InputSystem;
using DependencyLevel = BepInEx.BepInDependency.DependencyFlags;
using LoadedMods = BepInEx.Bootstrap.Chainloader;

namespace AutoWalk;
[BepInDependency(Plugin.LethalSettings, DependencyLevel.SoftDependency)]
[BepInDependency("com.rune580.LethalCompanyInputUtils")]
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal const string LethalSettings = "com.willis.lc.lethalsettings";
    internal static ManualLogSource Log;
    public static bool Enabled => LoadedMods.PluginInfos.ContainsKey(LethalSettings);
    internal static bool AutoWalkEnabled { get; set; }

    private void Awake()
    {
        Log = base.Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginInfo.PLUGIN_GUID);

        Keybinds.Instance.AutoWalkKey.performed += AutoWalkKey_performed;

        if (!Enabled)
        {
            return;
        }
        ModMenu.RegisterMod(new ModMenu.ModSettingsConfig
        {
            Name = GeneratedPluginInfo.Name,
            Id = GeneratedPluginInfo.Identifier,
            Version = GeneratedPluginInfo.Version,
            Description = "Adds a rebindable key for toggling auto walk so you can give your W-key finger a break!",
            MenuComponents =
            [
                new LabelComponent { Text = "Usage:"},
                new LabelComponent { FontSize = 10, Text = "Press the bound key (\"K\" by default) to toggle auto walking on or off.\nAuto walking will automatically disable if you manually input a walking direction." },
                new LabelComponent { Text = "\nRebinding Controls:", Alignment = TMPro.TextAlignmentOptions.BaselineLeft },
                new LabelComponent { FontSize = 10, Text = $"To change the key binding open the settings (in game or in main menu) and click \"Change keybinds\" and look for the item labeled \"Auto Walk\".\n\n{GeneratedPluginInfo.Name} does not ship with a preset binding for controllers out of the box, but the option to set a custom controller binding is there if you want it." }
            ]
        }, true, true);
    }

    private void AutoWalkKey_performed(InputAction.CallbackContext obj)
    {
        if (GameNetworkManager.Instance == null || GameNetworkManager.Instance.localPlayerController == null)
        {
            AutoWalkEnabled = false;
            return;
        }
        AutoWalkEnabled = !AutoWalkEnabled;
    }
}
