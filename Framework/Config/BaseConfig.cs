using Purps.Valheim.Framework.Data;

namespace Purps.Valheim.Framework.Config {
    public abstract class BaseConfig {
        public readonly GenericDictionary configDatas = new GenericDictionary();

        protected readonly BasePlugin plugin;

        protected BaseConfig(BasePlugin plugin) {
            this.plugin = plugin;
        }

        protected void ReadValueFromConfig<T>(ConfigData<T> configData) {
            configData.value = plugin.Config
                .Bind(configData.Section, configData.Key, configData.value, configData.Description).Value;

            configDatas.Add(configData.Key, configData);
        }
    }
}