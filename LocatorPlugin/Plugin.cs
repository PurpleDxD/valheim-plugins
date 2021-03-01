using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Purps.Valheim.Locator.Utils;
using Purps.Valheim.Utils;
using UnityEngine;

namespace Purps.Valheim.Locator {
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    [BepInProcess("valheim.exe")]
    public class Plugin : BaseUnityPlugin {
        private const string pluginGuid = "purps.valheim.locator";
        private const string pluginName = "Locator";
        private const string pluginVersion = "1.0.0";

        private const string description = "Finds and pins various Valheim locations / entities on the minimap!";
        private const string author = "Purps";

        public static int CastMask;

        public static CommandProcessor Processor;
        public static ConfigFile ConfigFile;

        private void Awake() {
            ConfigFile = Config;
            CastMask = LayerMask.GetMask("item", "player", "Default", "static_solid", "Default_small", "piece",
                "piece_nonsolid", "terrain", "character", "character_net", "character_ghost", "hitbox",
                "character_noenv", "vehicle");
            Processor = new CommandProcessor();
            CreateCommands();
            var harmony = new Harmony(pluginGuid);
            harmony.PatchAll();
        }

        private void Update() {
            MinimapUtils.Update();
        }

        private void OnDestroy() {
            var harmony = new Harmony(pluginGuid);
            harmony.UnpatchSelf();
            Processor.clearCommands();
            Processor = null;
            MinimapUtils.ClearTrackedComponents();
        }

        private static void CreateCommands() {
            Processor.addCommand(new Command("/locator-commands",
                "Displays all commands provided by the Locator plugin.",
                Processor.printCommands, false));

            Processor.addCommand(new Command("/locatemerchant", "Pins the BlackForest Merchant on your Minimap.",
                parameters => WorldUtils.Locate(Minimap.PinType.Icon3, new List<Tuple<string, string>> {
                    Tuple.Create("Vendor_BlackForest", "Merchant")
                }, false)));
            Processor.addCommand(new Command("/locatebosses", "Pins all boss altars on your Minimap.",
                parameters => WorldUtils.Locate(Minimap.PinType.Boss, new List<Tuple<string, string>> {
                    Tuple.Create("Eikthyrnir", "Eikthyr"),
                    Tuple.Create("GDKing", "The Elder"),
                    Tuple.Create("Bonemass", "Bonemass"),
                    Tuple.Create("Dragonqueen", "Moder"),
                    Tuple.Create("GoblinKing", "Yagluth")
                }, true)));

            Processor.addCommand(new Command("/listlocations",
                "Lists all the locations in the Console. Does not work on servers.",
                WorldUtils.ListLocations));
            Processor.addCommand(new Command("/listpins", "Lists all your Pins in the Console.",
                MinimapUtils.ListPins));
            Processor.addCommand(new Command("/clearpins", "Clears all your Pins.",
                MinimapUtils.ClearPins));

            Processor.addCommand(new Command("/autopin",
                "Toggles entity auto-pinning.", parameters => Locator.Config.AutoPin ^= true));

            Processor.addCommand(new Command("/pinspawners",
                "Toggles the pinning of spawners", parameters => Locator.Config.AutoPinSpawners ^= true));
            Processor.addCommand(new Command("/pinlocations",
                "Toggles the pinning of dungeons, caves, altars, runestones, etc.",
                parameters => Locator.Config.AutoPinLocations ^= true));
            Processor.addCommand(new Command("/pindestructibles",
                "Toggles the pinning of ores and berry bushes.",
                parameters => Locator.Config.AutoPinDestructibles ^= true));
            Processor.addCommand(new Command("/pinpickables",
                "Toggles the pinning of plans and fungi.", parameters => Locator.Config.AutoPinPickables ^= true));
            Processor.addCommand(new Command("/pinleviathans",
                "Toggles the pinning of leviathans.", parameters => Locator.Config.AutoPinLeviathans ^= true));

            Processor.addCommand(new Command("/pindistance",
                "The allowed distance between two entities for auto-pinning.", parameters => {
                    if (parameters.Length > 0 && int.TryParse(parameters[0], out var distance))
                        Locator.Config.AutoPinDistance = distance;
                }));
        }
    }
}