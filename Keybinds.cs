using LethalCompanyInputUtils.Api;
using UnityEngine.InputSystem;

namespace AutoWalk;

internal class Keybinds : LcInputActions
{
    public static Keybinds Instance = new();

    [InputAction("<Keyboard>/k", ActionId = "autowalkkey", Name = "Auto Walk")]
    public InputAction AutoWalkKey { get; set; }
}
