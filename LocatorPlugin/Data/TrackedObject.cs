namespace Purps.Valheim.Locator.Data {
    public class TrackedObject {
        public TrackedObject(string name, string pinName, bool shouldTrack) {
            Name = name;
            PinName = pinName;
            ShouldTrack = shouldTrack;
        }

        public string Name { get; set; }
        public string PinName { get; set; }
        public bool ShouldTrack { get; set; } = true;

        public static int QueryOrder(string name, string query) {
            if (name == query) return -1;
            return name.Contains(query) ? 0 : 1;
        }

        public override string ToString() {
            return $"{GetType().Name}[Name={Name}, PinName={PinName}, ShouldTrack={ShouldTrack}]";
        }
    }
}