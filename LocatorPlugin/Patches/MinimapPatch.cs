using System.Reflection;
using HarmonyLib;
using Purps.Valheim.Locator.Utils;
using UnityEngine;

namespace Purps.Valheim.Locator.Patches {
    [HarmonyPatch(typeof(Minimap), "OnMapRightClick")]
    public static class MinimapPatch {
        [HarmonyPostfix]
        internal static void Postfix(Minimap __instance) {
            var dynMethod = __instance.GetType()
                .GetMethod("ScreenToWorldPoint", BindingFlags.NonPublic | BindingFlags.Instance);
            var position = (Vector3) dynMethod.Invoke(__instance, new object[] {Input.mousePosition});
            MinimapUtils.RemovePin(position);
        }

        [HarmonyPatch(typeof(Minimap), "Awake")]
        [HarmonyPostfix]
        internal static void Awake(Minimap __instance) {
            MinimapUtils.ClearTrackedComponents();
        }
    }
}