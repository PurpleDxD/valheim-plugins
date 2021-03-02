using HarmonyLib;

namespace Purps.Valheim.SkipIntro.Patches {
    [HarmonyPatch(typeof(Player), "OnSpawned")]
    public class PlayerPatch {
        [HarmonyPrefix]
        internal static bool Prefix(bool ___m_firstSpawn) {
            if (!SkipIntroPlugin.Config.SkipIntro) return true;
            ___m_firstSpawn = false;
            return false;
        }
    }
}