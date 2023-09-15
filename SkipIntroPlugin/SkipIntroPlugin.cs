using BepInEx;
using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Commands;
using Purps.Valheim.Framework.Config;

namespace Purps.Valheim.SkipIntro {
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInProcess("valheim.exe")]
    public class SkipIntroPlugin : BasePlugin {
        private const string PluginGuid = "purps.valheim.skipintro";
        private const string PluginName = "Skip Intro";
        private const string PluginVersion = "1.1.0";

        public SkipIntroPlugin() : base(PluginGuid) { }

        public new static SkipIntroConfig Config => (SkipIntroConfig)BaseConfig;

        protected override void PluginAwake() {
            CreateCommands();
        }

        protected override void PluginDestroy() { }

        private static void CreateCommands() {
            CommandProcessor.AddCommand(new Command("/skipintro-commands",
                "Displays all commands provided by the SkipIntro plugin.", CommandProcessor.PrintCommands, false));
        }

        protected override BaseConfig GetConfig() {
            return new SkipIntroConfig(this);
        }
    }
}