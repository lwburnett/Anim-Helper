using Microsoft.Xna.Framework.Graphics;

namespace Anim_Helper.Utils;

internal struct ImportedImage
{
    public ImportedImage(string iPath, Texture2D iTexture)
    {
        Path = iPath;
        Texture = iTexture;
    }

    public string Path { get; }
    public Texture2D Texture { get; }
}