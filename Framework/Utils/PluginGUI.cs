using UnityEngine;

namespace Purps.Valheim.Framework.Utils {
    public static class PluginGUI {
        public static readonly GUIStyle Green = new GUIStyle();

        static PluginGUI() {
            Green.normal.textColor = Color.green;
            Green.fontSize = 15;
        }

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