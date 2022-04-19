using BepInEx;
using HarmonyLib;

namespace Purps.Valheim.LocateMerchant {
    [BepInPlugin("purps.valheim.locatemerchant", "Locate Merchant", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class Plugin : BaseUnityPlugin {
        private const string pluginGuid = "purps.valheim.locatemerchant";
        private const string pluginName = "Locate Merchant";
        private const string pluginVersion = "1.0.1";

        private void Awake() => new Harmony("purps.valheim.locatemerchant").PatchAll();
    }
}
