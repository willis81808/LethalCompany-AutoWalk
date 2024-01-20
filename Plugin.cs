using BepInEx.Logging;
using System;
using System.Reflection;
using LethalCompanyInputUtils.Api;
using UnityEngine.InputSystem;
using LethalSettings.UI;
using LethalSettings.UI.Components;

namespace AutoWalk;

[BepInDependency("com.willis.lc.lethalsettings")]
[BepInDependency("com.rune580.LethalCompanyInputUtils")]
[BepInPlugin(GeneratedPluginInfo.Identifier, GeneratedPluginInfo.Name, GeneratedPluginInfo.Version)]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log;

    internal static bool AutoWalkEnabled { get; set; }

    internal static Keybinds keybinds { get; private set; } = new Keybinds();

    private void Awake()
    {
        Log = base.Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GeneratedPluginInfo.Identifier);

        keybinds.AutoWalkKey.performed += AutoWalkKey_Pressed;

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

    private void AutoWalkKey_Pressed(InputAction.CallbackContext obj)
    {
        if (GameNetworkManager.Instance == null || GameNetworkManager.Instance.localPlayerController == null)
        {
            AutoWalkEnabled = false;
            return;
        }
        AutoWalkEnabled = !AutoWalkEnabled;
    }
}
