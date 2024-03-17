using System;
using System.Collections.Generic;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Anim_Helper.Controls;

internal class ParseTypeButtonControl : UiButtonBase
{
    public ParseTypeButtonControl(Rectangle iBounds)
    {
        HitBox = iBounds;
        OnClickedCallback = OnClicked;

        _isGrid = false;

        var dataSize = HitBox.Width * HitBox.Height;
        var colorDataDefault = new Color[dataSize];
        var colorDataHover = new Color[dataSize];
        var colorDataPress = new Color[dataSize];

        for (var ii = 0; ii < dataSize; ii++)
        {
            colorDataDefault[ii] = Settings.Colors.ButtonDefault;
            colorDataHover[ii] = Settings.Colors.ButtonHover;
            colorDataPress[ii] = Settings.Colors.ButtonPressed;
        }

        var dataDefault = GraphicsHelper.CreateTexture(colorDataDefault, HitBox.Width, HitBox.Height);
        var dataHover = GraphicsHelper.CreateTexture(colorDataHover, HitBox.Width, HitBox.Height);
        var dataPress = GraphicsHelper.CreateTexture(colorDataPress, HitBox.Width, HitBox.Height);

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

        var text = _isGrid ? cGridText : cFrameText;
        var stringDimensions = GraphicsHelper.GetFont().MeasureString(text);
        GraphicsHelper.DrawString(
            text,
            new Vector2(HitBox.X + (HitBox.Width - stringDimensions.X) / 2f, HitBox.Y + (HitBox.Height - stringDimensions.Y) / 2f),
            Color.Black);
    }

    protected sealed override Rectangle HitBox { get; set; }
    protected sealed override Action<GameTime> OnClickedCallback { get; set; }

    private bool _isGrid;
    private const string cFrameText = "Frame";
    private const string cGridText = "Grid";
    private readonly Dictionary<PressState, Texture2D> _backgroundTextures;

    private void OnClicked(GameTime iGameTime) => _isGrid = !_isGrid;
}