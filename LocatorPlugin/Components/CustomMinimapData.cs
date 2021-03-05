using Purps.Valheim.Framework;
using UnityEngine;

namespace Purps.Valheim.Locator.Components {
    public class CustomMinimapData : MonoBehaviour {
        public string[] PinFilters { get; set; } = BasePlugin.GetConfigData<string[]>("filterPins").value ?? null;
    }
}