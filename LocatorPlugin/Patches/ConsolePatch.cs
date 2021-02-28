using HarmonyLib;

namespace Purps.Valheim.Locator.Patches {
    [HarmonyPatch(typeof(Console), "InputText")]
    public class Console_InputText {
        [HarmonyPostfix]
        public static void Postfix(Console __instance) {
            var commandStr = __instance.m_input.text;
            if (!string.IsNullOrWhiteSpace(commandStr)) Plugin.Processor.executeCommand(__instance.m_input.text);
        }
    }
}