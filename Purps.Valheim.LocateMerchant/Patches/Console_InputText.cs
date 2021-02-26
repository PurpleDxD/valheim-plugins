using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace Purps.Valheim.LocateMerchant.Patches {
    [HarmonyPatch(typeof(Console), "InputText")]
    public class Console_InputText {
        private const Minimap.PinType pinType = Minimap.PinType.None;

        [HarmonyPostfix]
        public static void Postfix(Console __instance) {
            if (Player.m_localPlayer == null) return;
            if (__instance.m_input.text.ToUpper() != "LOCATEMERCHANT") return;

            Game.instance.DiscoverClosestLocation("Vendor_BlackForest", Player.m_localPlayer.transform.position,
                "Merchant", (int) pinType);
            var pinDatas = (List<Minimap.PinData>) Traverse.Create(Minimap.instance).Field("m_pins").GetValue();
            var pinData = pinDatas.First(p => p.m_type == pinType && p.m_name == "");
            Console.instance.Print($"Found BlackForest Merchant! Location: {pinData.m_pos}");
        }
    }
}