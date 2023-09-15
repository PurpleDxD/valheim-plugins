using HarmonyLib;
using Purps.Valheim.Framework;
using System.Linq;

namespace Purps.Valheim.LocateMerchant.Patches {
    [HarmonyPatch(typeof(Terminal), "TryRunCommand")]
    public class TerminalPatch
    {
        [HarmonyPrefix]
        internal static bool Prefix(Terminal __instance, string text)
        {
            if (BasePlugin.GetCommands().Any(command => text.StartsWith(command.Name)))
            {
                BasePlugin.ExecuteCommand(__instance.m_input.text);
                return false;
            }

            return true;
        }
    }
}
