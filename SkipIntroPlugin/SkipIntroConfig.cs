using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Config;
using Purps.Valheim.Framework.Utils;

namespace Purps.Valheim.SkipIntro {
    public class SkipIntroConfig : BaseConfig {
        private bool skipIntro;

        public SkipIntroConfig(BasePlugin plugin) : base(plugin) {
            skipIntro = plugin.Config.Bind("Default", "skip", true,
                "Defines whether or not the Valkyrie scene (intro) should be skipped.").Value;
        }

        public bool SkipIntro {
            get => skipIntro;
            set {
                skipIntro = value;
                ConsoleUtils.WriteToConsole($"SkipIntro: {SkipIntro}");
            }
        }
    }
}