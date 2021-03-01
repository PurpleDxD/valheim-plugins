using Purps.Valheim.Locator.Patches;

namespace Purps.Valheim.Locator {
    public static class Config {
        private static bool autoPin =
            Plugin.ConfigFile.Bind("AutoPin", "enabled", true, "Enables entity auto-pinning.").Value;

        private static bool autoPinSpawners = Plugin.ConfigFile
            .Bind("AutoPin", "pinSpawners", true, "Toggles the pinning of spawners.")
            .Value;

        private static bool autoPinLocations = Plugin.ConfigFile
            .Bind("AutoPin", "pinLocations", true, "Toggles the pinning of dungeons, caves, altars, runestones, etc.")
            .Value;

        private static bool autoPinDestructibles = Plugin.ConfigFile
            .Bind("AutoPin", "pinDestructibles", true, "Toggles the pinning of ores and berry bushes.")
            .Value;

        private static bool autoPinPickables = Plugin.ConfigFile
            .Bind("AutoPin", "pinPickables", true, "Toggles the pinning of plans and fungi.")
            .Value;

        private static bool autoPinLeviathans = Plugin.ConfigFile
            .Bind("AutoPin", "pinLeviathans", true, "Toggles the pinning of leviathans.")
            .Value;

        private static int autoPinDistance = Plugin.ConfigFile
            .Bind("AutoPin", "pinDistances", 30, "The allowed distance between two entities for auto-pinning.")
            .Value;

        public static bool AutoPin {
            get => autoPin;
            set {
                autoPin = value;
                ConsoleUtils.WriteToConsole($"AutoPin: {value}");
            }
        }

        public static bool AutoPinSpawners {
            get => autoPinSpawners;
            set {
                autoPinSpawners = value;
                ConsoleUtils.WriteToConsole($"Pin Spawners: {value}");
            }
        }

        public static bool AutoPinLocations {
            get => autoPinLocations;
            set {
                autoPinLocations = value;
                ConsoleUtils.WriteToConsole($"Pin Locations: {value}");
            }
        }

        public static bool AutoPinDestructibles {
            get => autoPinDestructibles;
            set {
                autoPinDestructibles = value;
                ConsoleUtils.WriteToConsole($"Pin Destructibles: {value}");
            }
        }

        public static bool AutoPinPickables {
            get => autoPinPickables;
            set {
                autoPinPickables = value;
                ConsoleUtils.WriteToConsole($"Pin Pickables: {value}");
            }
        }

        public static bool AutoPinLeviathans {
            get => autoPinLeviathans;
            set {
                autoPinLeviathans = value;
                ConsoleUtils.WriteToConsole($"Pin Leviathans: {value}");
            }
        }

        public static int AutoPinDistance {
            get => autoPinDistance;
            set {
                autoPinDistance = value;
                ConsoleUtils.WriteToConsole($"Pin Distance: {value}");
            }
        }
    }
}