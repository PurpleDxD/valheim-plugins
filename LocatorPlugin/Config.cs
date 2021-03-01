using BepInEx;
using BepInEx.Configuration;

namespace Purps.Valheim.Locator {
    public static class Config {
        public static readonly bool AutoPin =
            Plugin.ConfigFile.Bind("AutoPin", "enabled", true, "Enables entity auto-pinning.").Value;

        public static readonly bool AutoPinSpawners = Plugin.ConfigFile
            .Bind("AutoPin", "pinSpawners", true, "Toggles the pinning of spawners.")
            .Value;

        public static readonly bool AutoPinLocations = Plugin.ConfigFile
            .Bind("AutoPin", "pinLocations", true, "Toggles the pinning of dungeons, caves, altars, runestones, etc.")
            .Value;

        public static readonly bool AutoPinDestructibles = Plugin.ConfigFile
            .Bind("AutoPin", "pinDestructibles", true, "Toggles the pinning of ores and berry bushes.")
            .Value;

        public static readonly bool AutoPinPickables = Plugin.ConfigFile
            .Bind("AutoPin", "pinPickables", true, "Toggles the pinning of plans and fungi.")
            .Value;

        public static readonly bool AutoPinLeviathans = Plugin.ConfigFile
            .Bind("AutoPin", "pinLeviathans", true, "Toggles the pinning of leviathans.")
            .Value;

        public static readonly int AutoPinDistance = Plugin.ConfigFile
            .Bind("AutoPin", "pinDistances", 30, "The allowed distance between two entities for auto-pinning.")
            .Value;
    }
}