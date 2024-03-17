using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Anim_Helper.Utils;

internal class Sprite2D
{
    public Sprite2D(Texture2D iTexture, Rectangle iSourceRect)
    {
        Texture = iTexture;
        SourceRect = iSourceRect;
    }

    public Texture2D Texture { get; }
    public Rectangle SourceRect { get; }
}