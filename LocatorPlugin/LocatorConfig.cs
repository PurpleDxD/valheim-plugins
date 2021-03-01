using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Utils;

namespace Purps.Valheim.Locator {
    public class LocatorConfig : BaseConfig {
        private int _autoPinDistance;
        private bool autoPin;
        private bool autoPinDestructibles;
        private bool autoPinLeviathans;
        private bool autoPinLocations;
        private bool autoPinPickables;
        private bool autoPinSpawners;

        public LocatorConfig(BasePlugin plugin) : base(plugin) {
            autoPin = plugin.Config.Bind("AutoPin", "enabled", true,
                "Enables entity auto-pinning.").Value;
            autoPinSpawners = plugin.Config.Bind("AutoPin", "pinSpawners", true,
                "Toggles the pinning of spawners.").Value;
            autoPinLocations = plugin.Config.Bind("AutoPin", "pinLocations", true,
                "Toggles the pinning of dungeons, caves, altars, runestones, etc.").Value;
            autoPinDestructibles = plugin.Config.Bind("AutoPin", "pinDestructibles", true,
                "Toggles the pinning of ores and berry bushes.").Value;
            autoPinPickables = plugin.Config.Bind("AutoPin", "pinPickables", true,
                "Toggles the pinning of plans and fungi.").Value;
            autoPinLeviathans = plugin.Config.Bind("AutoPin", "pinLeviathans", true,
                "Toggles the pinning of leviathans.").Value;
            _autoPinDistance = plugin.Config.Bind("AutoPin", "pinDistances", 30,
                "The allowed distance between two entities for auto-pinning.").Value;
        }

        public bool AutoPin {
            get => autoPin;
            set {
                autoPin = value;
                ConsoleUtils.WriteToConsole($"AutoPin: {AutoPin}");
            }
        }

        public bool AutoPinSpawners {
            get => autoPinSpawners;
            set {
                autoPinSpawners = value;
                ConsoleUtils.WriteToConsole($"AutoPinSpawners: {AutoPinSpawners}");
            }
        }

        public bool AutoPinLocations {
            get => autoPinLocations;
            set {
                autoPinLocations = value;
                ConsoleUtils.WriteToConsole($"AutoPinLocations: {AutoPinLocations}");
            }
        }

        public bool AutoPinDestructibles {
            get => autoPinDestructibles;
            set {
                autoPinDestructibles = value;
                ConsoleUtils.WriteToConsole($"AutoPinDestructibles: {AutoPinDestructibles}");
            }
        }

        public bool AutoPinPickables {
            get => autoPinPickables;
            set {
                autoPinPickables = value;
                ConsoleUtils.WriteToConsole($"AutoPinPickables: {AutoPinPickables}");
            }
        }

        public bool AutoPinLeviathans {
            get => autoPinLeviathans;
            set {
                autoPinLeviathans = value;
                ConsoleUtils.WriteToConsole($"AutoPinLeviathans: {AutoPinLeviathans}");
            }
        }

        public int AutoPinDistance {
            get => _autoPinDistance;
            set {
                _autoPinDistance = value;
                ConsoleUtils.WriteToConsole($"AutoPinDistance: {AutoPinDistance}");
            }
        }
    }
}