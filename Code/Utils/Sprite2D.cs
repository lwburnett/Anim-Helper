using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Anim_Helper.Utils;

internal class Sprite2D
{
    public Sprite2D(string iLabel, Texture2D iTexture, Rectangle iSourceRect)
    {
        Label = iLabel;
        Texture = iTexture;
        SourceRect = iSourceRect;
    }

    public string Label { get; }
    public Texture2D Texture { get; }
    public Rectangle SourceRect { get; }
}