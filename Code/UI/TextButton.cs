using System;
using System.Collections.Generic;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Anim_Helper.UI;

internal class TextButton : UiButtonBase
{
    public TextButton(Rectangle iBounds, string iText, Action<GameTime> iOnClickedCallback) : base(iOnClickedCallback)
    {
        Bounds = iBounds;
        _text = iText;

        var dataSize = iBounds.Width * iBounds.Height;
        var colorDataDefault = new Color[dataSize];
        var colorDataHover = new Color[dataSize];
        var colorDataPress = new Color[dataSize];

        for (var ii = 0; ii < dataSize; ii++)
        {
            colorDataDefault[ii] = SettingsManager.ColorSettings.ButtonDefaultColor;
            colorDataHover[ii] = SettingsManager.ColorSettings.ButtonHoverColor;
            colorDataPress[ii] = SettingsManager.ColorSettings.ButtonPressedColor;
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

        _textFont = GraphicsHelper.LoadContent<SpriteFont>("PrototypeFont");
    }

    public override void Draw()
    {
        GraphicsHelper.DrawTexture(_backgroundTextures[StateOfPress], new Vector2(Bounds.X, Bounds.Y));

        var stringDimensions = _textFont.MeasureString(_text);
        GraphicsHelper.DrawString(
            _textFont,
            _text,
            new Vector2(Bounds.X + (Bounds.Width - stringDimensions.X) / 2f, Bounds.Y + (Bounds.Height - stringDimensions.Y) / 2f),
            Color.Black);
    }

    protected override Rectangle Bounds { get; }

    private readonly string _text;
    private readonly SpriteFont _textFont;
    private readonly Dictionary<PressState, Texture2D> _backgroundTextures;
}