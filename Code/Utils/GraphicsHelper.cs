using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;

namespace Anim_Helper.Utils;

internal class GraphicsHelper
{
    public static Texture2D CreateTexture(Color[] iColorData, int iWidth, int iHeight)
    {
        Debug.Assert(sGraphicsDevice != null);

        var texture = new Texture2D(sGraphicsDevice, iWidth, iHeight);
        texture.SetData(iColorData);
        return texture;
    }

    public static T LoadContent<T>(string iContentName)
    {
        Debug.Assert(sContentManager != null);

        return sContentManager.Load<T>(iContentName);
    }

    public static void DrawTexture(Texture2D iTexture, Vector2 iPosition, float iScale = 1.0f)
    {
        Debug.Assert(sSpriteBatch != null);
        sSpriteBatch.Draw(iTexture, iPosition, iTexture.Bounds, Color.White, 0.0f, Vector2.Zero, iScale, SpriteEffects.None, 1.0f);
    }


    public static void DrawString(string iText, Vector2 iPosition, Color iColor, float iScaling = 1.0f)
    {
        Debug.Assert(sSpriteBatch != null);
        sSpriteBatch.DrawString(sFont, iText, iPosition, iColor, 0.0f, Vector2.Zero, iScaling, SpriteEffects.None, 0.0f);
    }

    public static void RegisterGraphicsDevice(GraphicsDevice iGraphicsDevice)
    {
        Debug.Assert(sGraphicsDevice == null);
        sGraphicsDevice = iGraphicsDevice;
    }

    public static void RegisterSpriteBatch(SpriteBatch iSpriteBatch)
    {
        Debug.Assert(sSpriteBatch == null);
        sSpriteBatch = iSpriteBatch;
    }

    public static void RegisterContentManager(ContentManager iContentManager)
    {
        Debug.Assert(sContentManager == null);
        sContentManager = iContentManager;
        sFont = sContentManager.Load<SpriteFont>("PrototypeFont");
    }


    public static GraphicsDevice GetGraphicsDevice() => sGraphicsDevice;
    public static SpriteFont GetFont() => sFont;

    private static GraphicsDevice sGraphicsDevice;
    private static SpriteBatch sSpriteBatch;
    private static ContentManager sContentManager;

    private static SpriteFont sFont;
}