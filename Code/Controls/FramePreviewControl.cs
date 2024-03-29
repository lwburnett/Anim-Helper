﻿using System;
using System.IO;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Anim_Helper.Controls;

internal class FramePreviewControl : SelectableElementBase
{
    public FramePreviewControl(int iIndex, Vector2 iCenter, string iFramePath, Action<int, bool> iRequestMoveAction)
    {
        _center = iCenter;
        _framePreview = Texture2D.FromFile(GraphicsHelper.GetGraphicsDevice(), iFramePath);
        _label = Path.GetFileNameWithoutExtension(iFramePath);
        _requestMoveAction = iRequestMoveAction;

        var buttonHalfOffset = new Point((int)(Settings.Layout.Ribbon.FramePreview.ButtonSize.X / 2), 0);

        var leftButtonBounds = new Rectangle(
            iCenter.ToPoint() - Settings.Layout.Ribbon.FramePreview.ButtonOffset.ToPoint() - buttonHalfOffset,
            Settings.Layout.Ribbon.FramePreview.ButtonSize.ToPoint());
        _leftButton = new TextButton(leftButtonBounds, "<", _ => iRequestMoveAction(iIndex, false));

        var rightButtonBounds = new Rectangle(
            iCenter.ToPoint() + Settings.Layout.Ribbon.FramePreview.ButtonOffset.ToPoint() - buttonHalfOffset,
            Settings.Layout.Ribbon.FramePreview.ButtonSize.ToPoint());
        _rightButton = new TextButton(rightButtonBounds, ">", _ => iRequestMoveAction(iIndex, true));

        var controlWidth = (int)(Settings.Layout.Ribbon.FramePreview.ButtonOffset.X * 2 + Settings.Layout.Ribbon.FramePreview.ButtonSize.X);
        var controlHeight = (int)(Settings.Layout.Ribbon.FramePreview.ButtonOffset.X * 2 + Settings.Layout.Ribbon.FramePreview.ButtonSize.X / 2);

        HitBox = new Rectangle(
            (int)iCenter.X - controlWidth / 2, 
            (int)iCenter.Y - controlHeight / 2,
            controlWidth,
            controlHeight);

        _path = iFramePath;
    }

    public override void Update(GameTime iGameTime)
    {
        if (IsSelected)
        {
            _leftButton.Update(iGameTime);
            _rightButton.Update(iGameTime);
        }

        base.Update(iGameTime);
    }

    protected override void vDraw()
    {
        var textureScale = _framePreview.Height > _framePreview.Width ?
            Settings.Layout.Ribbon.FramePreview.SpriteDimensions.Y / _framePreview.Height :
            Settings.Layout.Ribbon.FramePreview.SpriteDimensions.X / _framePreview.Width;
        var texturePosition = _center - _framePreview.Bounds.Size.ToVector2() * textureScale / 2f;
        GraphicsHelper.DrawTexture(_framePreview, texturePosition, textureScale);

        var stringDimensions = GraphicsHelper.GetFont().MeasureString(_label);
        var labelScale = Settings.Layout.Ribbon.FramePreview.ButtonOffset.X * 2 / stringDimensions.X;

        var labelCenterLocation = _center + Settings.Layout.Ribbon.FramePreview.LabelOffset;
        GraphicsHelper.DrawString(
            _label,
            new Vector2(labelCenterLocation.X - stringDimensions.X * labelScale / 2f, labelCenterLocation.Y - stringDimensions.Y * labelScale / 2f),
            Color.Black,
            labelScale);

        if (IsSelected)
        {
            _leftButton.Draw();
            _rightButton.Draw();
        }
    }

    public void Move(int iNewIndex, Vector2 iNewCenter)
    {
        _center = iNewCenter;

        HitBox = new Rectangle(
            (int)iNewCenter.X - SelectTexture.Width / 2,
            (int)iNewCenter.Y - SelectTexture.Height / 2,
            SelectTexture.Width,
            SelectTexture.Height);

        var buttonHalfOffset = new Vector2((int)(Settings.Layout.Ribbon.FramePreview.ButtonSize.X / 2), 0);
        _leftButton.Move(iNewCenter - Settings.Layout.Ribbon.FramePreview.ButtonOffset - buttonHalfOffset, _ => _requestMoveAction(iNewIndex, false));
        _rightButton.Move(iNewCenter + Settings.Layout.Ribbon.FramePreview.ButtonOffset - buttonHalfOffset, _ => _requestMoveAction(iNewIndex, true));
    }

    public Texture2D GetSprite() => _framePreview;
    public string GetPath() => _path;

    private Vector2 _center;
    private readonly string _label;
    private readonly Action<int, bool> _requestMoveAction;

    private readonly Texture2D _framePreview;
    private readonly TextButton _leftButton;
    private readonly TextButton _rightButton;
    private readonly string _path;
}