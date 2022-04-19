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
        private const string PluginVersion = "1.0.1";

        private const string Description = "Skips the bird intro scene with new characters / maps.";
        private const string Author = "Purps";

        public SkipIntroPlugin() : base(PluginGuid) { }

        public new static SkipIntroConfig Config => (SkipIntroConfig)BaseConfig;

        protected override void PluginAwake() {
            CreateCommands();
        }

        protected override void PluginDestroy() { }

        private static void CreateCommands() {
            CommandProcessor.AddCommand(new Command("/skipintro-commands",
                "Displays all commands provided by the SkipIntro plugin.", CommandProcessor.PrintCommands, false));

            CommandProcessor.AddCommand(new Command("/skipintro",
                "Defines whether or not the Valkyrie scene (intro) should be skipped.",
                parameters => Config.SkipIntro ^= true));
        }

        protected override BaseConfig GetConfig() {
            return new SkipIntroConfig(this);
        }
    }
}