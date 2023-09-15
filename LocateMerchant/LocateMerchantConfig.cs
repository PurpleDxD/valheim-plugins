using Purps.Valheim.Framework;
using Purps.Valheim.Framework.Config;

namespace Purps.Valheim.LocateMerchant
{
    public class LocateMerchantConfig : BaseConfig {
        public LocateMerchantConfig(BasePlugin plugin) : base(plugin) { }

        protected override void handleCustomData<T>(ConfigData<T> configData)
        {
            return;
        }
    }
}