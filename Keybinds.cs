using LethalCompanyInputUtils.Api;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem;

namespace AutoWalk;

internal class Keybinds : LcInputActions
{
    [InputAction("<Keyboard>/k", ActionId = "autowalkkey", Name = "Auto Walk")]
    public InputAction AutoWalkKey { get; set; }
}
