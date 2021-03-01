using BepInEx;
using BepInEx.Configuration;
using Purps.Valheim.Framework.Commands;

namespace Purps.Valheim.Framework {
    public abstract class BasePlugin : BaseUnityPlugin {
        protected static ConfigFile ConfigFile;
        protected static CommandProcessor CommandProcessor;
        
        private void Awake() {
            ConfigFile = Config;
            CommandProcessor = new CommandProcessor();
            OnAwake();
        }

        protected abstract void OnAwake();
        protected abstract void OnDestroy();
    }
}