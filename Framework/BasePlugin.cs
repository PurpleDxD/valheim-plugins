using BepInEx;
using HarmonyLib;
using Purps.Valheim.Framework.Commands;
using Purps.Valheim.Framework.Config;

namespace Purps.Valheim.Framework {
    public abstract class BasePlugin : BaseUnityPlugin {
        protected static BaseConfig BaseConfig;
        public static CommandProcessor CommandProcessor;

        private readonly string PluginGuid;

        protected BasePlugin(string pluginGuid) {
            PluginGuid = pluginGuid;
        }

        private void Awake() {
            CommandProcessor = new CommandProcessor();
            BaseConfig = GetConfig();
            PluginAwake();
            var harmony = new Harmony(PluginGuid);
            harmony.PatchAll();
        }

        private void OnDestroy() {
            var harmony = new Harmony(PluginGuid);
            harmony.UnpatchSelf();
            BaseConfig.configDatas.Clear();
            BaseConfig = null;
            CommandProcessor.ClearCommands();
            CommandProcessor = null;
            PluginDestroy();
        }

        protected abstract BaseConfig GetConfig();

        protected abstract void PluginAwake();
        protected abstract void PluginDestroy();

        public static ConfigData<T> GetConfigData<T>(string key) {
            return BaseConfig.configDatas.GetValue<ConfigData<T>>(key);
        }

        public static void ExecuteCommand(string text) {
            CommandProcessor.ExecuteCommand(text);
        }
    }
}