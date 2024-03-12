using Microsoft.Xna.Framework;

namespace Anim_Helper.Utils
{
    internal static class SettingsManager
    {
        public static class LayoutSettings
        {
            public static Rectangle ImportButtonRect = new(20, 20, 100, 50);
        }

        public static class ColorSettings
        {
            public static Color ButtonDefaultColor = Color.LightGray;
            public static Color ButtonHoverColor = Color.DarkGray;
            public static Color ButtonPressedColor = Color.Gray;
        }
    }
}