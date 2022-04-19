using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Commands;
using Purps.Valheim.Framework.Config;
using Purps.Valheim.Locator.Data;
using Purps.Valheim.Locator.Exceptions;
using Purps.Valheim.Locator.Utils;

namespace Purps.Valheim.Locator {
    public class LocatorConfig : BaseConfig {
        public LocatorConfig(BasePlugin plugin) : base(plugin) {
            CreateCommandFromConfig(
                ReadValueFromConfig(
                    new ConfigData<bool>("General", "debug",
                        "Prints useful information to configure your own pinnable item types.", false)));

            CreateCommandFromConfig(
                ReadValueFromConfig(
                    new ConfigData<bool>("AutoPin", "pinEnabled",
                        "Enables entity auto-pinning.", true)));
            CreateCommandFromConfig(
                ReadValueFromConfig(
                    new ConfigData<float>("AutoPin", "pinDistance",
                        "The allowed distance between two entities for auto-pinning.", 30f)));
            CreateCommandFromConfig(
                ReadValueFromConfig(
                    new ConfigData<float>("AutoPin", "pinRayDistance",
                        "How close the to the entity the player must be for it to be auto-pinned.", 25f)));
            CreateCommandFromConfig(
                ReadValueFromConfig(
                    new ConfigData<bool>("AutoPin", "pinDestructibles",
                        "Toggles the pinning of destructible items.", true)));
            CreateCommandFromConfig(
                ReadValueFromConfig(
                    new ConfigData<bool>("AutoPin", "pinMineRocks",
                        "Toggles the pinning of mineable rocks.", true)));
            CreateCommandFromConfig(
                ReadValueFromConfig(
                    new ConfigData<bool>("AutoPin", "pinLocations",
                        "Toggles the pinning of dungeons, caves, altars, runestones, etc.", true)));
            CreateCommandFromConfig(
                ReadValueFromConfig(
                    new ConfigData<bool>("AutoPin", "pinPickables",
                        "Toggle the pinning of plants and fungi", true)));
            CreateCommandFromConfig(
                ReadValueFromConfig(
                    new ConfigData<bool>("AutoPin", "pinSpawners",
                        "Toggles the pinning of spawners.", true)));
            CreateCommandFromConfig(
                ReadValueFromConfig(
                    new ConfigData<bool>("AutoPin", "pinVegvisirs",
                        "Toggles the pinning of boss runestones.", true)));
            CreateCommandFromConfig(
                ReadValueFromConfig(
                    new ConfigData<bool>("AutoPin", "pinLeviathans",
                        "Toggles the pinning of leviathans.", true)));

            CreateCommandFromConfig(
                ReadValueFromConfig(
                    GetPinFilters(
                        "filterPins", "Default pin filters."), false), MinimapUtils.SetPinFilters);

            ReadValueFromConfig(
                GetInclusionList(
                    "destructibleInclusions",
                    "{silvervein,Silver,true}{rock3_silver,Silver,true}{MineRock_Tin,Tin,true}{rock4_copper,Copper,true}{MineRock_Obsidian,Obsidian,true}",
                    "Inclusion list for destructible items."), false);
            ReadValueFromConfig(
                GetInclusionList(
                    "mineRockInclusions",
                    "{MineRock_Meteorite,Meteorite,true}",
                    "Inclusion list for minable rocks."), false);
            ReadValueFromConfig(
                GetInclusionList(
                    "locationInclusions",
                    "{DrakeLorestone,Runestone,true}{TrollCave,Troll,true}{Crypt,Crypt,true}{SunkenCrypt,Crypt,true}{Grave,Grave,true}{DrakeNest,Egg,true}{Runestone,Runestone,true}{Eikthyrnir,Eikthyr,true}{GDKing,The Elder,true}{Bonemass,Bonemass,true}{Dragonqueen,Moder,true}{GoblinKing,Yagluth,true}",
                    "Inclusion list for locations."), false);
            ReadValueFromConfig(
                GetInclusionList(
                    "pickableInclusions",
                    "{BlueberryBush,BlueBerry,true}{CloudberryBush,Cloudberry,true}{RaspberryBush,Raspberry,true}{Pickable_Barley,Barley,true}{Pickable_Flax,Flax,true}{Pickable_Thistle,Thistle,true}{Pickable_Mushroom,Mushroom,true}{Pickable_SeedCarrot,Carrot,true}{Pickable_Dandelion,Dandelion,true}{Pickable_SeedTurnip,Turnip,true}",
                    "Inclusion list for pickable items."), false);
            ReadValueFromConfig(
                GetInclusionList(
                    "spawnerInclusions",
                    "{Spawner,Spawner,true}",
                    "Inclusion list for spawners."), false);
            ReadValueFromConfig(
                GetInclusionList(
                    "vegvisirInclusions",
                    "{Vegvisir,Runestone,true}",
                    "Inclusion list for boss runestones."), false);
            ReadValueFromConfig(
                GetInclusionList(
                    "leviathanInclusions",
                    "{Leviathan,Leviathan,true}",
                    "Inclusion list for leviathans."), false);
        }

        private static void CreateCommandFromConfig<T>(ConfigData<T> configData, Action<string[]> action = null) {
            BasePlugin.CommandProcessor.AddCommand(new Command(
                $"/{configData.Key}", configData.Description,
                action ?? (parameters => SetValue(configData, parameters))));
        }

        private static void SetValue<T>(ConfigData<T> configData, string[] parameters) {
            switch (configData.value) {
                case bool value:
                    value ^= true;
                    configData.value = (T) (object) value;
                    break;
                case float value:
                    if (parameters.Length > 0f && float.TryParse(parameters[0], out var parsedParameter))
                        configData.value = (T) (object) parsedParameter;
                    break;
                case string[] value:
                    MinimapUtils.SetPinFilters(value);
                    break;
            }
        }

        private ConfigData<string[]> GetPinFilters(string name, string description) {
            var configString = plugin.Config.Bind("Pins", name, "", description).Value;

            string[] value = null;

            if (!string.IsNullOrWhiteSpace(configString))
                value = string.Join(" ", configString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries))
                    .Split(' ');

            return new ConfigData<string[]>("Pins", name, description, value);
        }

        private ConfigData<List<TrackedObject>> GetInclusionList(string name, string defaultValue,
            string description) {
            var configString = plugin.Config.Bind("Inclusions", name, "", description).Value;

            if (string.IsNullOrWhiteSpace(configString)) configString = defaultValue;

            var inclusionsDataList = new Regex(@"\b[A-Za-z-'_, 0-9]+\b").Matches(configString)
                .Cast<Match>()
                .Select(m => m.Groups[0].Value)
                .ToList();

            var inclusionsList = inclusionsDataList.Select(i => GetInclusionObject(name, i)).ToList();

            return new ConfigData<List<TrackedObject>>("Inclusions", name, description, inclusionsList);
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