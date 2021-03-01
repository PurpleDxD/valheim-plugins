using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Commands;

namespace Purps.Valheim.SkipIntro {
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    [BepInProcess("valheim.exe")]
    public class SkipIntroPlugin : BasePlugin {
        private const string pluginGuid = "purps.valheim.skipintro";
        private const string pluginName = "Skip Intro";
        private const string pluginVersion = "1.0.0";

        private const string description = "Skips the bird intro scene with new characters / maps.";
        private const string author = "Purps";

        public static CommandProcessor Processor;
        public new static SkipIntroConfig Config;

        protected override void OnAwake() {
            Processor = new CommandProcessor();
            Config = new SkipIntroConfig(this);
            CreateCommands();
            var harmony = new Harmony(pluginGuid);
            harmony.PatchAll();
        }

        protected override void OnDestroy() {
            Config = null;
            Processor.clearCommands();
            Processor = null;
            var harmony = new Harmony(pluginGuid);
            harmony.UnpatchSelf();
        }

        private static void CreateCommands() {
            Processor.addCommand(new Command("/skipintro-commands",
                "Displays all commands provided by the SkipIntro plugin.",
                Processor.printCommands, false));

            Processor.addCommand(new Command("/skipintro",
                "Defines whether or not the Valkyrie scene (intro) should be skipped.",
                parameters => Config.SkipIntro ^= true));
        }
    }
}