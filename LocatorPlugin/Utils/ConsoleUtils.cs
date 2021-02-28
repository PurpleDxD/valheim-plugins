using System.Linq;

namespace Purps.Valheim.Locator.Patches {
    public static class ConsoleUtils {
        public static void WriteToConsole(params string[] textElements) {
            var str = "";
            if (textElements.Length == 1)
                str = textElements.First();
            else if (textElements.Length > 1)
                str = textElements.Aggregate(str, (current, text) => current + text + " ");

            if (str != "") Console.instance.Print(str);
        }
    }
}