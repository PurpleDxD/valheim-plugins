namespace Purps.Valheim.Framework.Config {
    public class ConfigData<T>  {
        public readonly string Description;
        public readonly string Key;
        public readonly string Section;
        public T value;

        public ConfigData(string section, string key, string description, T value) {
            Section = section;
            Key = key;
            Description = description;
            this.value = value;
        }
    }
}