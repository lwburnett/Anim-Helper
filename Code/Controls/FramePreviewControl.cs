﻿using System;
using System.IO;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Anim_Helper.Controls;

internal class FramePreviewControl : IGameElement
{
    public FramePreviewControl(int iIndex, Vector2 iCenter, string iFramePath, Action<int, bool> iRequestMoveAction)
    {
        _center = iCenter;
        _framePreview = Texture2D.FromFile(GraphicsHelper.GetGraphicsDevice(), iFramePath);
        _label = Path.GetFileNameWithoutExtension(iFramePath);

        var leftButtonBounds = new Rectangle(
            iCenter.ToPoint() - Settings.Layout.Ribbon.FramePreview.ButtonOffset.ToPoint(),
            Settings.Layout.Ribbon.FramePreview.ButtonSize.ToPoint());
        _leftButton = new TextButton(leftButtonBounds, "<", _ => iRequestMoveAction(iIndex, false));

        var rightButtonBounds = new Rectangle(
            iCenter.ToPoint() + Settings.Layout.Ribbon.FramePreview.ButtonOffset.ToPoint(),
            Settings.Layout.Ribbon.FramePreview.ButtonSize.ToPoint());
        _rightButton = new TextButton(rightButtonBounds, ">", _ => iRequestMoveAction(iIndex, true));
    }

    public void Update(GameTime iGameTime)
    {
        _leftButton.Update(iGameTime);
        _rightButton.Update(iGameTime);
    }

    public void Draw()
    {
        var textureScale = _framePreview.Height > _framePreview.Width ?
            Settings.Layout.Ribbon.FramePreview.SpriteDimensions.X / _framePreview.Width :
            Settings.Layout.Ribbon.FramePreview.SpriteDimensions.Y / _framePreview.Height;
        var texturePosition = _center - _framePreview.Bounds.Size.ToVector2() * textureScale / 2f;
        GraphicsHelper.DrawTexture(_framePreview, texturePosition, textureScale);

        var stringDimensions = GraphicsHelper.GetFont().MeasureString(_label);
        var labelCenterLocation = _center + Settings.Layout.Ribbon.FramePreview.LabelOffset;
        GraphicsHelper.DrawString(
            _label,
            new Vector2(labelCenterLocation.X - stringDimensions.X / 2f, labelCenterLocation.Y - stringDimensions.Y / 2f),
            Color.Black);

        _leftButton.Draw();
        _rightButton.Draw();
    }

    private Vector2 _center;
    private readonly string _label;

    private readonly Texture2D _framePreview;
    private readonly IGameElement _leftButton;
    private readonly IGameElement _rightButton;
}