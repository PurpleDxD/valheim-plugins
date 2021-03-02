using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Utils;
using Purps.Valheim.Locator.Exceptions;
using UnityEngine;

namespace Purps.Valheim.Locator {
    public class LocatorConfig : BaseConfig {
        private bool debug;
        private bool autoPin;
        private float autoPinDistance;
        private float autoPinRayDistance;

        private bool autoPinDestructibles;
        private List<TrackedObject> destructibleInclusions;

        private bool autoPinMineRocks;
        private List<TrackedObject> mineRockInclusions;

        private bool autoPinLocations;
        private List<TrackedObject> locationInclusions;

        private bool autoPinPickables;
        private List<TrackedObject> pickableInclusions;

        private bool autoPinSpawners;
        private List<TrackedObject> spawnerInclusions;

        private bool autoPinVegvisirs;
        private List<TrackedObject> vegvisirInclusions;

        private bool autoPinLeviathans;
        private List<TrackedObject> leviathanInclusions;

        public LocatorConfig(BasePlugin plugin) : base(plugin) {
            debug = plugin.Config.Bind("General", "debug", false,
                "Prints useful information to configure your own pinnable item types.").Value;
            autoPin = plugin.Config.Bind("AutoPin", "enabled", true,
                "Enables entity auto-pinning.").Value;
            autoPinDistance = plugin.Config.Bind("AutoPin", "pinDistances", 30f,
                "The allowed distance between two entities for auto-pinning.").Value;
            autoPinRayDistance = plugin.Config.Bind("AutoPin", "pinRayDistance", 25f,
                "How close the to the entity the player must be for it to be auto-pinned.").Value;

            autoPinDestructibles = plugin.Config.Bind("AutoPin", "pinDestructibles", true,
                "Toggles the pinning of destructible items.").Value;
            autoPinMineRocks = plugin.Config.Bind("AutoPin", "pinMineRocks", true,
                "Toggles the pinning of mineable rocks.").Value;
            autoPinLocations = plugin.Config.Bind("AutoPin", "pinLocations", true,
                "Toggles the pinning of dungeons, caves, altars, runestones, etc.").Value;
            autoPinPickables = plugin.Config.Bind("AutoPin", "pinPickables", true,
                "Toggles the pinning of plants and fungi.").Value;
            autoPinSpawners = plugin.Config.Bind("AutoPin", "pinSpawners", true,
                "Toggles the pinning of spawners.").Value;
            autoPinVegvisirs = plugin.Config.Bind("AutoPin", "pinVegvisirs", true,
                "Toggles the pinning of boss runestones.").Value;
            autoPinLeviathans = plugin.Config.Bind("AutoPin", "pinLeviathans", true,
                "Toggles the pinning of leviathans.").Value;

            destructibleInclusions =
                GetInclusionList(
                    "destructibleInclusions",
                    "{silvervein,Silver,true}{rock3_silver,Silver,true}{BlueberryBush,BlueBerry,true}{CloudberryBush,Cloudberry,true}{RaspberryBush,Raspberry,true}{MineRock_Tin,Tin,true}{rock4_copper,Copper,true}{MineRock_Obsidian,Obsidian,true}",
                    "Inclusion list for destructible items.");
            mineRockInclusions =
                GetInclusionList(
                    "mineRockInclusions",
                    "{MineRock_Meteorite,Meteorite,true}",
                    "Inclusion list for minable rocks.");
            locationInclusions =
                GetInclusionList(
                    "locationInclusions",
                    "{DrakeLorestone,Runestone,true}{TrollCave,BlueBerry,true}{Crypt,Crypt,true}{SunkenCrypt,Crypt,true}{Grave,Grave,true}{DrakeNest,Egg,true}{Runestone,Runestone,true}{Eikthyrnir,Eikthyr,true}{GDKing,The Elder,true}{Bonemass,Bonemass,true}{Dragonqueen,Moder,true}{GoblinKing,Yagluth,true}",
                    "Inclusion list for locations.");
            pickableInclusions =
                GetInclusionList(
                    "pickableInclusions",
                    "{Pickable_Barley,Barley,true}{Pickable_Flax,Flax,true}{Pickable_Thistle,Thistle,true}{Pickable_Mushroom,Mushroom,true}{Pickable_SeedCarrot,Carrot,true}{Pickable_Dandelion,Dandelion,true}{Pickable_SeedTurnip,Turnip,true}",
                    "Inclusion list for pickable items.");
            spawnerInclusions =
                GetInclusionList(
                    "spawnerInclusions",
                    "{Spawner,Spawner,true}",
                    "Inclusion list for spawners.");
            vegvisirInclusions =
                GetInclusionList(
                    "vegvisirInclusions",
                    "{Vegvisir,Vegvisir,true}",
                    "Inclusion list for boss runestones.");
            leviathanInclusions =
                GetInclusionList(
                    "spawnerInclusions",
                    "{Leviathan,Leviathan,true}",
                    "Inclusion list for leviathans.");
        }

        private List<TrackedObject> GetInclusionList(string name, string defaultValue, string description) {
            var configString = plugin.Config.Bind("Inclusions", name, "", description).Value;

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

        public bool Debug {
            get => debug;
            set {
                debug = value;
                ConsoleUtils.WriteToConsole($"Debug: {Debug}");
            }
        }


        public bool AutoPin {
            get => autoPin;
            set {
                autoPin = value;
                ConsoleUtils.WriteToConsole($"AutoPin: {AutoPin}");
            }
        }

        public float AutoPinDistance {
            get => autoPinDistance;
            set {
                autoPinDistance = value;
                ConsoleUtils.WriteToConsole($"AutoPinDistance: {AutoPinDistance}");
            }
        }

        public float AutoPinRayDistance {
            get => autoPinRayDistance;
            set {
                autoPinRayDistance = value;
                ConsoleUtils.WriteToConsole($"AutoPinRayDistance: {autoPinRayDistance}");
            }
        }

        public bool AutoPinDestructibles {
            get => autoPinDestructibles;
            set {
                autoPinDestructibles = value;
                ConsoleUtils.WriteToConsole($"AutoPinDestructibles: {AutoPinDestructibles}");
            }
        }

        public List<TrackedObject> DestructibleInclusions {
            get => destructibleInclusions;
            set {
                destructibleInclusions = value;
                ConsoleUtils.WriteToConsole(string.Join(", ", DestructibleInclusions));
            }
        }

        public bool AutoPinMineRocks {
            get => autoPinMineRocks;
            set {
                autoPinMineRocks = value;
                ConsoleUtils.WriteToConsole($"AutoPinMineRocks: {autoPinMineRocks}");
            }
        }

        public List<TrackedObject> MineRockInclusions {
            get => mineRockInclusions;
            set {
                mineRockInclusions = value;
                ConsoleUtils.WriteToConsole(string.Join(", ", MineRockInclusions));
            }
        }

        public bool AutoPinLocations {
            get => autoPinLocations;
            set {
                autoPinLocations = value;
                ConsoleUtils.WriteToConsole($"AutoPinLocations: {AutoPinLocations}");
            }
        }

        public List<TrackedObject> LocationInclusions {
            get => locationInclusions;
            set {
                locationInclusions = value;
                ConsoleUtils.WriteToConsole(string.Join(", ", locationInclusions));
            }
        }

        public bool AutoPinPickables {
            get => autoPinPickables;
            set {
                autoPinPickables = value;
                ConsoleUtils.WriteToConsole($"AutoPinPickables: {AutoPinPickables}");
            }
        }

        public List<TrackedObject> PickableInclusions {
            get => pickableInclusions;
            set {
                pickableInclusions = value;
                ConsoleUtils.WriteToConsole(string.Join(", ", pickableInclusions));
            }
        }

        public bool AutoPinSpawners {
            get => autoPinSpawners;
            set {
                autoPinSpawners = value;
                ConsoleUtils.WriteToConsole($"AutoPinSpawners: {AutoPinSpawners}");
            }
        }

        public List<TrackedObject> SpawnerInclusions {
            get => spawnerInclusions;
            set {
                spawnerInclusions = value;
                ConsoleUtils.WriteToConsole(string.Join(", ", spawnerInclusions));
            }
        }

        public bool AutoPinVegvisirs {
            get => autoPinVegvisirs;
            set {
                autoPinVegvisirs = value;
                ConsoleUtils.WriteToConsole($"AutoPinVegvisirs: {autoPinVegvisirs}");
            }
        }

        public List<TrackedObject> VegvisirInclusions {
            get => vegvisirInclusions;
            set {
                vegvisirInclusions = value;
                ConsoleUtils.WriteToConsole(string.Join(", ", vegvisirInclusions));
            }
        }

        public bool AutoPinLeviathans {
            get => autoPinLeviathans;
            set {
                autoPinLeviathans = value;
                ConsoleUtils.WriteToConsole($"AutoPinLeviathans: {AutoPinLeviathans}");
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