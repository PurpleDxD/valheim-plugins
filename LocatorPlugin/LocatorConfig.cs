using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Config;
using Purps.Valheim.Locator.Data;
using Purps.Valheim.Locator.Exceptions;

namespace Purps.Valheim.Locator {
    public class LocatorConfig : BaseConfig {
        public LocatorConfig(BasePlugin plugin) : base(plugin) {
            ReadValueFromConfig(
                new ConfigData<bool>("General", "debug",
                    "Prints useful information to configure your own pinnable item types.", false));
            ReadValueFromConfig(
                new ConfigData<bool>("AutoPin", "pinEnabled",
                    "Enables entity auto-pinning.", true));
            ReadValueFromConfig(
                new ConfigData<float>("AutoPin", "pinDistance",
                    "The allowed distance between two entities for auto-pinning.", 30f));
            ReadValueFromConfig(
                new ConfigData<float>("AutoPin", "pinRayDistance",
                    "How close the to the entity the player must be for it to be auto-pinned.", 25f));

            ReadValueFromConfig(
                new ConfigData<bool>("AutoPin", "pinDestructibles",
                    "Toggles the pinning of destructible items.", true));
            ReadValueFromConfig(
                new ConfigData<bool>("AutoPin", "pinMineRocks",
                    "Toggles the pinning of mineable rocks.", true));
            ReadValueFromConfig(
                new ConfigData<bool>("AutoPin", "pinLocations",
                    "Toggles the pinning of dungeons, caves, altars, runestones, etc.", true));
            ReadValueFromConfig(
                new ConfigData<bool>("AutoPin", "pinPickables",
                    "Toggle the pinning of plants and fungi", true));
            ReadValueFromConfig(
                new ConfigData<bool>("AutoPin", "pinSpawners",
                    "Toggles the pinning of spawners.", true));
            ReadValueFromConfig(
                new ConfigData<bool>("AutoPin", "pinVegvisirs",
                    "Toggles the pinning of boss runestones.", true));
            ReadValueFromConfig(
                new ConfigData<bool>("AutoPin", "pinLeviathans",
                    "Toggles the pinning of leviathans.", true));

            DestructibleInclusions =
                GetInclusionList(
                    "destructibleInclusions",
                    "{silvervein,Silver,true}{rock3_silver,Silver,true}{BlueberryBush,BlueBerry,true}{CloudberryBush,Cloudberry,true}{RaspberryBush,Raspberry,true}{MineRock_Tin,Tin,true}{rock4_copper,Copper,true}{MineRock_Obsidian,Obsidian,true}",
                    "Inclusion list for destructible items.");
            MineRockInclusions =
                GetInclusionList(
                    "mineRockInclusions",
                    "{MineRock_Meteorite,Meteorite,true}",
                    "Inclusion list for minable rocks.");
            LocationInclusions =
                GetInclusionList(
                    "locationInclusions",
                    "{DrakeLorestone,Runestone,true}{TrollCave,BlueBerry,true}{Crypt,Crypt,true}{SunkenCrypt,Crypt,true}{Grave,Grave,true}{DrakeNest,Egg,true}{Runestone,Runestone,true}{Eikthyrnir,Eikthyr,true}{GDKing,The Elder,true}{Bonemass,Bonemass,true}{Dragonqueen,Moder,true}{GoblinKing,Yagluth,true}",
                    "Inclusion list for locations.");
            PickableInclusions =
                GetInclusionList(
                    "pickableInclusions",
                    "{Pickable_Barley,Barley,true}{Pickable_Flax,Flax,true}{Pickable_Thistle,Thistle,true}{Pickable_Mushroom,Mushroom,true}{Pickable_SeedCarrot,Carrot,true}{Pickable_Dandelion,Dandelion,true}{Pickable_SeedTurnip,Turnip,true}",
                    "Inclusion list for pickable items.");
            SpawnerInclusions =
                GetInclusionList(
                    "spawnerInclusions",
                    "{Spawner,Spawner,true}",
                    "Inclusion list for spawners.");
            VegvisirInclusions =
                GetInclusionList(
                    "vegvisirInclusions",
                    "{Vegvisir,Vegvisir,true}",
                    "Inclusion list for boss runestones.");
            LeviathanInclusions =
                GetInclusionList(
                    "spawnerInclusions",
                    "{Leviathan,Leviathan,true}",
                    "Inclusion list for leviathans.");
        }

        public List<TrackedObject> DestructibleInclusions { get; }
        public List<TrackedObject> MineRockInclusions { get; }
        public List<TrackedObject> LocationInclusions { get; }
        public List<TrackedObject> PickableInclusions { get; }
        public List<TrackedObject> SpawnerInclusions { get; }
        public List<TrackedObject> VegvisirInclusions { get; }
        public List<TrackedObject> LeviathanInclusions { get; }

        private List<TrackedObject> GetInclusionList(string name, string defaultValue, string description) {
            var configString = plugin.Config.Bind("Inclusions", name, "", description).Value;

            if (string.IsNullOrWhiteSpace(configString)) configString = defaultValue;

            var inclusionsDataList = new Regex(@"\b[A-Za-z-'_, 0-9]+\b").Matches(configString)
                .Cast<Match>()
                .Select(m => m.Groups[0].Value)
                .ToList();
            return inclusionsDataList.Select(i => GetInclusionObject(name, i)).ToList();
        }

        private static TrackedObject GetInclusionObject(string name, string data) {
            var properties = data.Split(',');
            if (properties.Length == 3)
                if (bool.TryParse(properties[2], out var shouldTrack))
                    return new TrackedObject(properties[0], properties[1], shouldTrack);

            throw new MappingException($"Failed to load property {name}.");
        }
    }
}