using HarmonyLib;

namespace Purps.Valheim.Locator.Patches {
    [HarmonyPatch(typeof(Chat), "InputText")]
    public class ChatPatch {
        [HarmonyPostfix]
        public static void Postfix(Chat __instance) {
            var commandStr = __instance.m_input.text;
            if (!string.IsNullOrWhiteSpace(commandStr)) Plugin.Processor.executeCommand(__instance.m_input.text);
        }
    }
}