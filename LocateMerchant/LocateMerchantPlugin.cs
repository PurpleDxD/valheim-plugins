using BepInEx;
using HarmonyLib;
using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Commands;
using Purps.Valheim.Framework.Config;
using Purps.Valheim.Locator.Utils;
using System;
using System.Collections.Generic;

namespace Purps.Valheim.LocateMerchant {
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInProcess("valheim.exe")]
    public class LocateMerchantPlugin : BasePlugin  {
        private const string PluginGuid = "purps.valheim.locatemerchant";
        private const string PluginName = "Locate Merchant";
        private const string PluginVersion = "1.1.0";

        public LocateMerchantPlugin() : base(PluginGuid) { }

        public new static LocateMerchantConfig Config => (LocateMerchantConfig)BaseConfig;

        protected override void PluginAwake()
        {
            CreateCommands();
        }

        protected override void PluginDestroy() { }

        protected void CreateCommands()
        {
            CommandProcessor.AddCommand(new Command("/locatemerchant-commands",
                "Displays all commands provided by the Locator plugin.", CommandProcessor.PrintCommands, false));

            CommandProcessor.AddCommand(new Command("/locatemerchant", "Pins the BlackForest Merchant on your Minimap.",
                 parameters => WorldUtils.Locate(Minimap.PinType.Icon3, new List<Tuple<string, string>> {
                    Tuple.Create("Vendor_BlackForest", "Merchant")
                 }, false)));
        }

        protected override BaseConfig GetConfig()
        {
            return new LocateMerchantConfig(this);
        }
    }
}
