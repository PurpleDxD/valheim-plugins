using Purps.Valheim.Framework.Commands;
using Purps.Valheim.Framework.Data;
using Purps.Valheim.Locator.Utils;
using System;

namespace Purps.Valheim.Framework.Config {
    public abstract class BaseConfig {
        public readonly GenericDictionary configDatas = new GenericDictionary();

        protected readonly BasePlugin plugin;

        protected BaseConfig(BasePlugin plugin) {
            this.plugin = plugin;
        }

        protected abstract void handleCustomData<T>(ConfigData<T> configData);

        protected ConfigData<T> ReadValueFromConfig<T>(ConfigData<T> configData, bool shouldFetch = true) {
            if (shouldFetch)
                configData.value = plugin.Config
                    .Bind(configData.Section, configData.Key, configData.value, configData.Description).Value;

            configDatas.Add(configData.Key, configData);

            return configData;
        }

        protected void CreateCommandFromConfig<T>(ConfigData<T> configData, Action<string[]> action = null)
        {
            BasePlugin.CommandProcessor.AddCommand(new Command(
                $"/{configData.Key}", configData.Description,
                action ?? (parameters => SetValue(configData, parameters))));
        }

        private void SetValue<T>(ConfigData<T> configData, string[] parameters)
        {
            switch (configData.value)
            {
                case bool value:
                    value ^= true;
                    configData.value = (T)(object)value;
                    break;
                case float value:
                    if (parameters.Length > 0f && float.TryParse(parameters[0], out var parsedParameter))
                        configData.value = (T)(object)parsedParameter;
                    break;
                default:
                    handleCustomData(configData);
                    break;
            }
        }
    }
}