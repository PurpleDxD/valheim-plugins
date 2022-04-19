using HarmonyLib;
using Purps.Valheim.Framework;

namespace Purps.Valheim.SkipIntro.Patches {
    [HarmonyPatch(typeof(Chat), "InputText")]
    public class ChatPatch {
        [HarmonyPostfix]
        internal static void Postfix(Chat __instance) {
            var commandStr = __instance.m_input.text;
            if (!string.IsNullOrWhiteSpace(commandStr))
                BasePlugin.ExecuteCommand(__instance.m_input.text);
        }
    }
}