using System;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using Purps.Valheim.Locator.Utils;
using Purps.Valheim.Utils;

namespace Purps.Valheim.Locator {
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    [BepInProcess("valheim.exe")]
    public class Plugin : BaseUnityPlugin {
        private const string pluginGuid = "purps.valheim.locator";
        private const string pluginName = "Locator";
        private const string pluginVersion = "1.0.0";

        private const string description = "Finds and pins various Valheim locations / entities on the minimap!";
        private const string author = "Purps";

        public static readonly CommandProcessor Processor = new CommandProcessor();

        private void Awake() {
            CreateCommands();
            var harmony = new Harmony(pluginGuid);
            harmony.PatchAll();
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
            Processor.addCommand(new Command("/listpins", "Lists all the Pins in the Console.",
                WorldUtils.ListPins));
        }
    }
}