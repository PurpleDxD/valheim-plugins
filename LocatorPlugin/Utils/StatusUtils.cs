using Purps.Valheim.Framework.Utils;

namespace Purps.Valheim.Locator.Utils {
    public static class StatusUtils {
        public static bool IsPlayerLoaded() {
            if (Player.m_localPlayer != null) return true;
            ConsoleUtils.WriteToConsole("Player must be in-game for commands to work!");
            return false;
        }

        public static bool IsPlayerOffline() {
            if (ZNet.instance.GetConnectedPeers().Count == 0) return true;
            ConsoleUtils.WriteToConsole("This command does not work on a server.");
            return false;
        }
    }
}