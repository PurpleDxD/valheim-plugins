using System.Collections.Generic;

namespace Purps.Valheim.Framework.Data {

    public static class PluginContainer {
        private static readonly Dictionary<string, object> Container = new Dictionary<string, object>();

        public static void Add<T>(string key, T value) {
            if (!Container.TryGetValue(key, out _)) {
                Container.Add(key, value);
            } else {
                Container[key] = value;
            }
        }

        public static T Get<T>(string key) {
            return (T) Container[key];
        }
    }
}