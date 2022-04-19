using BepInEx;
using HarmonyLib;
using Purps.Valheim.Framework.Utils;
using System;
using System.Collections.Generic;

namespace Purps.Valheim.Framework {

    public abstract class BasePlugin : BaseUnityPlugin {
        private readonly string PluginGuid;

        protected BasePlugin(string pluginGuid) {
            PluginGuid = pluginGuid;
        }

        private void Awake() {
            PluginAwake();
            var harmony = new Harmony(PluginGuid);
            harmony.PatchAll();
        }

        private void OnDestroy() {
            var harmony = new Harmony(PluginGuid);
            harmony.UnpatchSelf();
            PluginLogger.Destroy();
            PluginDestroy();
        }

        protected abstract void PluginAwake();

        protected abstract void PluginDestroy();
    }
}