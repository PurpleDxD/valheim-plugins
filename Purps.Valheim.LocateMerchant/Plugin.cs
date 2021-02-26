using BepInEx;
using HarmonyLib;

namespace Purps.Valheim.LocateMerchant {
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    [BepInProcess("valheim.exe")]
    public class Plugin : BaseUnityPlugin {
        private const string pluginGuid = "purps.valheim.locatemerchant";
        private const string pluginName = "Locate Merchant";
        private const string pluginVersion = "1.0.0";

        private void Awake() {
            var harmony = new Harmony(pluginGuid);
            harmony.PatchAll();
        }
    }
}