using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Config;
using Purps.Valheim.Framework.Utils;

namespace Purps.Valheim.SkipIntro {
    public class SkipIntroConfig : BaseConfig {
        public SkipIntroConfig(BasePlugin plugin) : base(plugin) {
            CreateCommandFromConfig(
                ReadValueFromConfig(
                    new ConfigData<bool>("Default", "skip",
                        "Defines whether or not the Valkyrie scene (intro) should be skipped.", true)));
        }

        protected override void handleCustomData<T>(ConfigData<T> configData)
        {
            return;
        }
    }
}