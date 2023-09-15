using HarmonyLib;
using Purps.Valheim.Framework;

namespace Purps.Valheim.SkipIntro.Patches {
    [HarmonyPatch(typeof(Player), "OnSpawned")]
    public class PlayerPatch {
        [HarmonyPrefix]
        internal static bool Prefix(bool ___m_firstSpawn) {
            if (!BasePlugin.GetConfigData<bool>("skip").value) return true;
            ___m_firstSpawn = false;
            return false;
        }
    }
}