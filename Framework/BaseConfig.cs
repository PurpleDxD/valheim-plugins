namespace Purps.Valheim.Framework {
    public abstract class BaseConfig {
        protected readonly BasePlugin plugin;

        protected BaseConfig(BasePlugin plugin) {
            this.plugin = plugin;
        }
    }
}