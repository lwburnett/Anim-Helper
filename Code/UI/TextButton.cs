using System;
using System.Collections.Generic;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Anim_Helper.UI;

internal class TextButton : UiButtonBase
{
    public TextButton(Rectangle iBounds, string iText, Action<GameTime> iOnClickedCallback)
    {
        HitBox = iBounds;
        _text = iText;
        OnClickedCallback = iOnClickedCallback;

        var dataSize = iBounds.Width * iBounds.Height;
        var colorDataDefault = new Color[dataSize];
        var colorDataHover = new Color[dataSize];
        var colorDataPress = new Color[dataSize];

        for (var ii = 0; ii < dataSize; ii++)
        {
            colorDataDefault[ii] = Settings.Colors.ButtonDefault;
            colorDataHover[ii] = Settings.Colors.ButtonHover;
            colorDataPress[ii] = Settings.Colors.ButtonPressed;
        }

        var dataDefault = GraphicsHelper.CreateTexture(colorDataDefault, iBounds.Width, iBounds.Height);
        var dataHover = GraphicsHelper.CreateTexture(colorDataHover, iBounds.Width, iBounds.Height);
        var dataPress = GraphicsHelper.CreateTexture(colorDataPress, iBounds.Width, iBounds.Height);

        _backgroundTextures = new Dictionary<PressState, Texture2D>
        {
            { PressState.Default, dataDefault },
            { PressState.Hover, dataHover },
            { PressState.Pressed, dataPress }
        };
    }

    public override void Draw()
    {
        GraphicsHelper.DrawTexture(_backgroundTextures[StateOfPress], new Vector2(HitBox.X, HitBox.Y));

        var stringDimensions = GraphicsHelper.GetFont().MeasureString(_text);
        GraphicsHelper.DrawString(
            _text,
            new Vector2(HitBox.X + (HitBox.Width - stringDimensions.X) / 2f, HitBox.Y + (HitBox.Height - stringDimensions.Y) / 2f),
            Color.Black);
    }

    public void Move(Vector2 iNewPosition, Action<GameTime> iNewOnClickedCallback)
    {
        HitBox = new Rectangle(iNewPosition.ToPoint(), HitBox.Size);
        OnClickedCallback = iNewOnClickedCallback;
    }

    protected sealed override Rectangle HitBox { get; set; }

    protected sealed override Action<GameTime> OnClickedCallback { get; set; }

    private readonly string _text;
    private readonly Dictionary<PressState, Texture2D> _backgroundTextures;
}