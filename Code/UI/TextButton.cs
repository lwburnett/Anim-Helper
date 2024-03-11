using System;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;

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
    }

    public override void Draw()
    {
        throw new NotImplementedException();
    }

    protected override Rectangle Bounds { get; }

    private readonly string _text;
}