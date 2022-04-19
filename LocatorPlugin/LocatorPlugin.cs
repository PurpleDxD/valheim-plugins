using BepInEx;
using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Data;
using Purps.Valheim.Framework.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Purps.Valheim.Locator {

    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInProcess("valheim.exe")]
    public class LocatorPlugin : BasePlugin {
        private const string PluginGuid = "purps.valheim.locator";
        private const string PluginName = "Locator";
        private const string PluginVersion = "1.1.0";

        public LocatorPlugin() : base(PluginGuid) {
            PluginContainer.Add("debugText", "");
            PluginContainer.Add("tracked", new List<Type> {
                typeof(Destructible),
                typeof(MineRock),
                typeof(Location),
                typeof(Pickable),
                typeof(SpawnArea),
                typeof(Vegvisir),
                typeof(Leviathan)
            });
        }

        private void OnGUI() {
            PluginGUI.DrawOutline(new Rect(10, 5, Screen.width, 20), PluginContainer.Get<string>("debugText"), PluginGUI.Green);
        }

        protected override void PluginAwake() {
        }

        protected override void PluginDestroy() {
        }
    }
}