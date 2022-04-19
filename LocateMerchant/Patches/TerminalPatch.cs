using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Purps.Valheim.LocateMerchant.Patches {
    [HarmonyPatch(typeof(Terminal), "InputText")]
    public class TerminalPatch {
        private const Minimap.PinType pinType = Minimap.PinType.None;

        [HarmonyPostfix]
        public static void Postfix(Console __instance) {
            if ((UnityEngine.Object)Player.m_localPlayer == (UnityEngine.Object)null || __instance.m_input.text.ToUpper() != "LOCATEMERCHANT")
                return;
            Game.instance.DiscoverClosestLocation("Vendor_BlackForest", Player.m_localPlayer.transform.position, "Merchant", 8);
            Console.instance.Print(string.Format("Found BlackForest Merchant! Location: {0}", (object)((IEnumerable<Minimap.PinData>)Traverse.Create((object)Minimap.instance).Field("m_pins").GetValue()).First<Minimap.PinData>((Func<Minimap.PinData, bool>)(p => p.m_type == Minimap.PinType.None && p.m_name == "")).m_pos));
        }
    }
}
