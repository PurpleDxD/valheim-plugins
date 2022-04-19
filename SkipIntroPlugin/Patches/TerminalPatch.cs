using HarmonyLib;
using Purps.Valheim.Framework;

namespace Purps.Valheim.SkipIntro.Patches {
    [HarmonyPatch(typeof(Terminal), "InputText")]
    public class TerminalPatch {
        [HarmonyPostfix]
        internal static void Postfix(Terminal __instance) {
            var commandStr = __instance.m_input.text;
            if (!string.IsNullOrWhiteSpace(commandStr))
                BasePlugin.ExecuteCommand(__instance.m_input.text);
        }
    }
}