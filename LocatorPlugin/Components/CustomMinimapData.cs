using UnityEngine;

namespace Purps.Valheim.Locator.Components {
    public class CustomMinimapData : MonoBehaviour {
        public string[] PinFilters { get; set; } = LocatorPlugin.Config.PinFilters ?? null;
    }
}