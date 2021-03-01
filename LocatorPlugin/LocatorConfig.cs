using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Utils;
using Purps.Valheim.Locator.Exceptions;
using UnityEngine;

namespace Purps.Valheim.Locator {
    public class LocatorConfig : BaseConfig {
        private bool autoPin;
        private int autoPinDistance;

        private bool autoPinDestructibles;
        private List<TrackedObject> destructibleInclusions;

        private bool autoPinLocations;
        private List<TrackedObject> locationInclusions;

        private bool autoPinPickables;
        private List<TrackedObject> pickableInclusions;

        private bool autoPinSpawners;
        private List<TrackedObject> spawnerInclusions;

        private bool autoPinLeviathans;
        private List<TrackedObject> leviathanInclusions;

        public LocatorConfig(BasePlugin plugin) : base(plugin) {
            autoPin = plugin.Config.Bind("AutoPin", "enabled", true,
                "Enables entity auto-pinning.").Value;
            autoPinDistance = plugin.Config.Bind("AutoPin", "pinDistances", 30,
                "The allowed distance between two entities for auto-pinning.").Value;

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

            destructibleInclusions =
                GetInclusionList(
                    "destructibleExclusions",
                    "{BlueberryBush,BlueBerry,true},{CloudberryBush,Cloudberry,true},{RaspberryBush,Raspberry,true},{MineRock_Tin,Tin,true},{rock4_copper,Copper,true},{MineRock_Obsidian,Obsidian,true}",
                    "Exclusion list for destructible items.");
            locationInclusions =
                GetInclusionList(
                    "locationInclusions",
                    "{TrollCave,BlueBerry,true},{Crypt,Crypt,true},{SunkenCrypt,Crypt,true},{Grave,Grave,true},{DrakeNest,Egg,true},{Runestone,Runestone,true},{Eikthyrnir,Eikthyr,true},{GDKing,The Elder,true},{Bonemass,Bonemass,true},{Dragonqueen,Moder,true},{GoblinKing,Yagluth,true}",
                    "Exclusion list for locations.");
            pickableInclusions =
                GetInclusionList(
                    "pickableInclusions",
                    "{Pickable_Thistle,Thistle,true},{Pickable_Mushroom,Mushroom,true},{Pickable_SeedCarrot,Carrot,true},{Pickable_Dandelion,Dandelion,true},{Pickable_SeedTurnip,Turnip,true}",
                    "Exclusion list for pickable items.");
            spawnerInclusions =
                GetInclusionList(
                    "spawnerInclusions",
                    "{Spawner,Spawner,true}",
                    "Exclusion list for spawners.");
            leviathanInclusions =
                GetInclusionList(
                    "spawnerInclusions",
                    "{Leviathan,Leviathan,true}",
                    "Exclusion list for leviathans.");
        }

        private List<TrackedObject> GetInclusionList(string name, string defaultValue, string description) {
            var configString = plugin.Config.Bind("Exclusions", name, "", description).Value;

            if (string.IsNullOrWhiteSpace(configString)) {
                configString = defaultValue;
            }

            var inclusionsDataList = new Regex(@"\b[A-Za-z-'_, 0-9]+\b").Matches(configString)
                .Cast<Match>()
                .Select(m => m.Groups[0].Value)
                .ToList();
            return inclusionsDataList.Select(i => GetTrackedObject(name, i)).ToList();
        }

        private static TrackedObject GetTrackedObject(string name, string data) {
            var properties = data.Split(',');
            if (properties.Length == 3) {
                if (bool.TryParse(properties[2], out var shouldTrack)) {
                    return new TrackedObject(properties[0], properties[1], shouldTrack);
                }
            }

            throw new MappingException($"Failed to load property {name}.");
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
            get => autoPinDistance;
            set {
                autoPinDistance = value;
                ConsoleUtils.WriteToConsole($"AutoPinDistance: {AutoPinDistance}");
            }
        }
        
        public List<TrackedObject> DestructibleInclusions {
            get => destructibleInclusions;
            set {
                destructibleInclusions = value;
                ConsoleUtils.WriteToConsole(string.Join(", ", DestructibleInclusions));
            }
        }

        public List<TrackedObject> LocationInclusions {
            get => locationInclusions;
            set {
                locationInclusions = value;
                ConsoleUtils.WriteToConsole(string.Join(", ", locationInclusions));
            }
        }

        public List<TrackedObject> PickableInclusions {
            get => pickableInclusions;
            set {
                pickableInclusions = value;
                ConsoleUtils.WriteToConsole(string.Join(", ", pickableInclusions));
            }
        }

        public List<TrackedObject> SpawnerInclusions {
            get => spawnerInclusions;
            set {
                spawnerInclusions = value;
                ConsoleUtils.WriteToConsole(string.Join(", ", spawnerInclusions));
            }
        }
        
        public List<TrackedObject> LeviathanInclusions {
            get => leviathanInclusions;
            set {
                leviathanInclusions = value;
                ConsoleUtils.WriteToConsole(string.Join(", ", leviathanInclusions));
            }
        }
    }
}