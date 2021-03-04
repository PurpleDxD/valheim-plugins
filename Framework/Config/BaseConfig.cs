using Purps.Valheim.Framework.Data;
using UnityEngine;

namespace Purps.Valheim.Framework.Config {
    public abstract class BaseConfig {
        public readonly GenericDictionary configDatas = new GenericDictionary();

        protected readonly BasePlugin plugin;

        protected BaseConfig(BasePlugin plugin) {
            this.plugin = plugin;
        }

        protected ConfigData<T> ReadValueFromConfig<T>(ConfigData<T> configData, bool shouldFetch = true) {
            if (shouldFetch)
                configData.value = plugin.Config
                    .Bind(configData.Section, configData.Key, configData.value, configData.Description).Value;

            configDatas.Add(configData.Key, configData);

            return configData;
        }
    }
}