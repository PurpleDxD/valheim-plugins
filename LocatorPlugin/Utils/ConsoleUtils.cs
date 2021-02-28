namespace Purps.Valheim.Locator.Patches {
    public static class ConsoleUtils {
        public static void WriteToConsole(string text) {
            Console.instance.Print(text);
        }
    }
}