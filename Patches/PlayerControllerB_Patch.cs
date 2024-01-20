using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace AutoWalk.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerB_Patch
{
    [HarmonyTranspiler]
    [HarmonyPatch("Update")]
    internal static IEnumerable<CodeInstruction> Update(IEnumerable<CodeInstruction> instructions, ILGenerator ilGenerator)
    {
        var codes = new List<CodeInstruction>(instructions);
        var startIndex = codes.FindIndex(c => c.opcode == OpCodes.Ldstr && (string)c.operand == "Move");

        if (startIndex == -1)
        {
            Plugin.Log.LogError("Failed to find movement instruction to replace! Falling back to vanilla behaviour.");
            return codes;
        }

        codes.RemoveRange(startIndex - 3, 7);
        var method = AccessTools.Method(typeof(PlayerControllerB_Patch), nameof(GetRealOrSimulatedMovement));
        codes.Insert(startIndex - 3, new CodeInstruction(OpCodes.Call, method));

        //codes.GetRange(startIndex - 3, 15).Do(c => Plugin.Log.LogWarning(c.ToString()));

        return codes.AsEnumerable();
    }

    public static Vector2 GetRealOrSimulatedMovement()
    {
        var defaultValue = IngamePlayerSettings.Instance.playerInput.actions.FindAction("Move", false).ReadValue<Vector2>();

        if (defaultValue.sqrMagnitude > 0.01f || !Plugin.AutoWalkEnabled)
        {
            Plugin.AutoWalkEnabled = false;
            return defaultValue;
        }
        else
        {
            return Vector2.up;
        }
    }
}
