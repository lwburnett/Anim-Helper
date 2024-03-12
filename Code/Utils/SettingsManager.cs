﻿using Microsoft.Xna.Framework;

namespace Anim_Helper.Utils
{
    internal static class Settings
    {
        public static class Layout
        {
            public static class Ribbon
            {
                public static Rectangle ImportButtonRect = new(20, 20, 100, 50);

                public static Vector2 FrameFirstPosition = new(150, 150);
                public const int FrameSpacingX = 180;

                public static class FramePreview
                {
                    public static Vector2 SpriteDimensions = new(100, 100);
                    public static Vector2 LabelOffset = new(0, 70);
                    public static Vector2 ButtonOffset = new(70, 0);
                    public static Vector2 ButtonSize = new(20, 20);
                }
            }


        }

        public static class Colors
        {
            public static Color ButtonDefault = Color.LightGray;
            public static Color ButtonHover = Color.DarkGray;
            public static Color ButtonPressed = Color.Gray;
        }
    }
}