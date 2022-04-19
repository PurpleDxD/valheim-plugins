using UnityEngine;

namespace Purps.Valheim.Locator.Utils {
    public static class GuiUtils {
        public static void DrawOutline(Rect position, string text, GUIStyle style) {
            var originalColor = style.normal.textColor;
            style.normal.textColor = Color.black;
            position.x--;
            GUI.Label(position, text, style);
            position.x += 2;
            GUI.Label(position, text, style);
            position.x--;
            position.y--;
            GUI.Label(position, text, style);
            position.y += 2;
            GUI.Label(position, text, style);
            position.y--;
            style.normal.textColor = originalColor;
            GUI.Label(position, text, style);
        }
    }
}