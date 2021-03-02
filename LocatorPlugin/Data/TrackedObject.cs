namespace Purps.Valheim.Locator {
    public class TrackedObject {
        public string Name { get; set; }
        public string PinName { get; set; }
        public bool ShouldTrack { get; set; } = true;

        public TrackedObject(string name, string pinName, bool shouldTrack) {
            Name = name;
            PinName = pinName;
            ShouldTrack = shouldTrack;
        }

        public override string ToString() {
            return $"{GetType().Name}[Name={Name}, PinName={PinName}, ShouldTrack={ShouldTrack}]";
        }
    }
}