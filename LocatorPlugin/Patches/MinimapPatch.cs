using System.Reflection;
using HarmonyLib;
using Purps.Valheim.Locator.Components.Utils;
using UnityEngine;

namespace Purps.Valheim.Locator.Components.Patches {
    [HarmonyPatch(typeof(Minimap), "OnMapRightClick")]
    public static class MinimapPatch {
        [HarmonyPostfix]
        internal static void Postfix(Minimap __instance) {
            var dynMethod = __instance.GetType()
                .GetMethod("ScreenToWorldPoint", BindingFlags.NonPublic | BindingFlags.Instance);
            var position = (Vector3) dynMethod.Invoke(__instance, new object[] {Input.mousePosition});
            MinimapUtils.RemovePin(position);
        }

        [HarmonyPatch(typeof(Minimap), "UpdatePins")]
        [HarmonyPostfix]
        internal static void UpdatePins(Minimap __instance) {
            MinimapUtils.FilterPins();
        }
        
        [HarmonyPatch(typeof(Minimap), "Awake")]
        [HarmonyPostfix]
        internal static void Awake(Minimap __instance) {
            MinimapUtils.OnAwake();
        }

        [HarmonyPatch(typeof(Minimap), "OnDestroy")]
        [HarmonyPostfix]
        internal static void OnDestroy(Minimap __instance) {
            MinimapUtils.OnDestroy();
        }
        
    }
}