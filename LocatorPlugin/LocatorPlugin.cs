using System;
using System.Collections.Generic;
using BepInEx;
using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Commands;
using Purps.Valheim.Framework.Config;
using Purps.Valheim.Locator.Components.Utils;
using UnityEngine;

namespace Purps.Valheim.Locator.Components {
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInProcess("valheim.exe")]
    public class LocatorPlugin : BasePlugin {
        private const string PluginGuid = "purps.valheim.locator";
        private const string PluginName = "Locator";
        private const string PluginVersion = "1.0.0";

        private const string Description = "Finds and pins various Valheim locations / entities on the minimap!";
        private const string Author = "Purps";

        public static readonly int CastMask = LayerMask.GetMask("item", "player", "Default", "static_solid",
            "Default_small",
            "piece", "piece_nonsolid", "terrain", "character", "character_net", "character_ghost", "hitbox",
            "character_noenv", "vehicle");

        private static readonly GUIStyle DebugStyle = new GUIStyle();
        public static string DebugText = "";
        public static Rect DebugTextLocation = new Rect(10, 5, Screen.width, 20);

        public LocatorPlugin() : base(PluginGuid) { }

        public new static LocatorConfig Config => (LocatorConfig) BaseConfig;

        private void Update() {
            DebugText = "";
            MinimapUtils.Update();
        }

        private void OnGUI() {
            GuiUtils.DrawOutline(DebugTextLocation, DebugText, DebugStyle);
        }

        protected override void PluginAwake() {
            DebugStyle.normal.textColor = Color.green;
            DebugStyle.fontSize = 15;
            CreateCommands();
            MinimapUtils.OnAwake();
        }

        protected override void PluginDestroy() {
            MinimapUtils.OnDestroy();
        }

        private static void CreateCommands() {
            CommandProcessor.AddCommand(new Command("/locator-commands",
                "Displays all commands provided by the Locator plugin.", CommandProcessor.PrintCommands, false));

            CommandProcessor.AddCommand(new Command("/locatemerchant", "Pins the BlackForest Merchant on your Minimap.",
                parameters => WorldUtils.Locate(Minimap.PinType.Icon3, new List<Tuple<string, string>> {
                    Tuple.Create("Vendor_BlackForest", "Merchant")
                }, false)));
            CommandProcessor.AddCommand(new Command("/locatebosses", "Pins all boss altars on your Minimap.",
                parameters => WorldUtils.Locate(Minimap.PinType.Boss, new List<Tuple<string, string>> {
                    Tuple.Create("Eikthyrnir", "Eikthyr"),
                    Tuple.Create("GDKing", "The Elder"),
                    Tuple.Create("Bonemass", "Bonemass"),
                    Tuple.Create("Dragonqueen", "Moder"),
                    Tuple.Create("GoblinKing", "Yagluth")
                }, true)));

            CommandProcessor.AddCommand(new Command("/listlocations",
                "Lists all the locations in the Console. Does not work on servers.",
                WorldUtils.ListLocations));

            CommandProcessor.AddCommand(new Command("/listpins", "Lists all your Pins in the Console.",
                MinimapUtils.ListPins));
            CommandProcessor.AddCommand(new Command("/clearpins", "Clears all your Pins.",
                MinimapUtils.ClearPins));
            CommandProcessor.AddCommand(new Command("/pinfilters", "", MinimapUtils.SetPinFilters));

            CreateCommandFromConfig(GetConfigData<bool>("debug"));
            CreateCommandFromConfig(GetConfigData<bool>("pinEnabled"));
            CreateCommandFromConfig(GetConfigData<float>("pinDistance"));
            CreateCommandFromConfig(GetConfigData<float>("pinRayDistance"));
            CreateCommandFromConfig(GetConfigData<bool>("pinDestructibles"));
            CreateCommandFromConfig(GetConfigData<bool>("pinMineRocks"));
            CreateCommandFromConfig(GetConfigData<bool>("pinLocations"));
            CreateCommandFromConfig(GetConfigData<bool>("pinPickables"));
            CreateCommandFromConfig(GetConfigData<bool>("pinSpawners"));
            CreateCommandFromConfig(GetConfigData<bool>("pinVegvisirs"));
            CreateCommandFromConfig(GetConfigData<bool>("pinLeviathans"));
        }

        private static void CreateCommandFromConfig<T>(ConfigData<T> configData) {
            CommandProcessor.AddCommand(new Command(
                $"/{configData.Key}", configData.Description, parameters => SetValue(configData, parameters)));
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
                default:
                    throw new NotSupportedException($"Type {configData.value} is not supported.");
            }
        }

        protected override BaseConfig GetConfig() {
            return new LocatorConfig(this);
        }
    }
}