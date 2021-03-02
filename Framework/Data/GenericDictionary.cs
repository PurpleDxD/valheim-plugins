using System.Collections.Generic;

namespace Purps.Valheim.Framework.Data {
    public class GenericDictionary {
        private readonly Dictionary<string, object> Dictionary = new Dictionary<string, object>();

        public Dictionary<string, object>.KeyCollection Keys => Dictionary.Keys;
        
        public void Add<T>(string key, T value) where T : class {
            Dictionary.Add(key, value);
        }

        public object GetValue(string key) {
            return Dictionary[key];
        }
        
        public T GetValue<T>(string key) where T : class {
            return Dictionary[key] as T;
        }

        public void Clear() {
            Dictionary.Clear();
        }
    }
}