using HarmonyLib;
using UnityEngine;

namespace Purps.Valheim.LocateMerchant.Patches {
    
    [HarmonyPatch(typeof(Console), "InputText")]
    public class Console_InputText {
        [HarmonyPostfix]
        public static void Postfix(Console __instance) {
            if (Player.m_localPlayer == null) return;
            if (__instance.m_input.text.ToUpper() != "LOCATEMERCHANT") return;
            ZoneSystem.instance.FindClosestLocation("Vendor_BlackForest", Player.m_localPlayer.transform.position, out var locationInstance);
            Minimap.instance.DiscoverLocation(locationInstance.m_position, Minimap.PinType.Icon3, "Merchant");
            Console.instance.Print($"Found BlackForest Merchant! Location: {locationInstance.m_position}");
        }
    }
    
}