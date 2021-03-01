using HarmonyLib;
using Purps.Valheim.SkipIntro;

namespace Purps.Valheim.SkipIntro.Patches {
    [HarmonyPatch(typeof(Console), "InputText")]
    public class ConsolePatch {
        [HarmonyPostfix]
        internal static void Postfix(Console __instance) {
            var commandStr = __instance.m_input.text;
            if (!string.IsNullOrWhiteSpace(commandStr)) SkipIntroPlugin.Processor.executeCommand(__instance.m_input.text);
        }
    }
}