using System;
using System.Collections.Generic;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Anim_Helper.Controls;

internal class GridParsePreviewControl : SelectableElementBase
{
    public GridParsePreviewControl(Action<bool> iOnSelectionChanged)
    {
        HitBox = new Rectangle();
        _image = null;
        _onSelectionChanged = iOnSelectionChanged;

        _topLeft = new Vector2(400);
        _scale = 1.0f;
        _lastMousePosition = null;
        _lastScrollWheelValue = null;

        _previews = new List<PreviewRectangle>();
    }

    public void SetImage(Texture2D iImage)
    {
        if (iImage == null)
            return;

        _image = iImage;

        HitBox = new Rectangle(_topLeft.ToPoint(), new Point(_image.Width, _image.Height));
    }

    public void SetPreviewBounds(List<Rectangle> iPreviewBounds)
    {
        _previews.Clear();

        foreach (var previewBound in iPreviewBounds)
        {
            var thisCellColorData = new Color[previewBound.Width * previewBound.Height];

            for (var yy = 0; yy < previewBound.Height; yy++)
            for (var xx = 0; xx < previewBound.Width; xx++)
            {
                var thisPixelNum = xx + yy * previewBound.Width;

                if (xx < Settings.Layout.GridPreview.CellLineWidth || previewBound.Width - xx <= Settings.Layout.GridPreview.CellLineWidth ||
                    yy < Settings.Layout.GridPreview.CellLineWidth || previewBound.Height - yy <= Settings.Layout.GridPreview.CellLineWidth)
                    thisCellColorData[thisPixelNum] = Settings.Colors.GridPreview;
                else
                    thisCellColorData[thisPixelNum] = Color.Transparent;
            }

            var thisCellTexture = GraphicsHelper.CreateTexture(thisCellColorData, previewBound.Width, previewBound.Height);
            var thisCellPosition = previewBound.Location.ToVector2();

            _previews.Add(new PreviewRectangle(thisCellPosition, thisCellTexture));
        }
    }

    public override void Update(GameTime iGameTime)
    {
        if (_image == null)
            return;

        // Handle click and drag
        var mouseState = Mouse.GetState();
        if (IsSelected && IsOverlappingWithMouse(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
        {
            if (_lastMousePosition.HasValue)
            {
                var mouseDiff = mouseState.Position - _lastMousePosition.Value;
                _topLeft += mouseDiff.ToVector2();
                HitBox = new Rectangle(_topLeft.ToPoint(), new Point(HitBox.Width, HitBox.Height));
            }

            _lastMousePosition = mouseState.Position;
        }

        if (_lastMousePosition.HasValue && ((IsSelected && mouseState.LeftButton == ButtonState.Released) || !IsSelected))
            _lastMousePosition = null;

        // Handle mouse wheel
        if (IsSelected && IsOverlappingWithMouse(mouseState.Position))
        {
            if (_lastScrollWheelValue.HasValue)
            {
                var scrollDiff = _lastScrollWheelValue.Value - mouseState.ScrollWheelValue;

                if (scrollDiff != 0)
                {
                    _scale += scrollDiff * Settings.Layout.Flipbook.ScaleChangePerWheelTick;

                    var width = _image.Width;
                    var height = _image.Height;
                    HitBox = new Rectangle(HitBox.Location, (new Vector2(width, height) * _scale).ToPoint());
                }
            }

            _lastScrollWheelValue = mouseState.ScrollWheelValue;
        }
        else if (_lastScrollWheelValue.HasValue)
            _lastScrollWheelValue = null;

        base.Update(iGameTime);
    }

    protected override void vDraw()
    {
        if (_image == null)
            return;

        GraphicsHelper.DrawTexture(_image, _topLeft, _scale);

        foreach (var previewTexture in _previews)
        {
            var position = _topLeft + previewTexture.Position * _scale;

            GraphicsHelper.DrawTexture(previewTexture.Texture, position, _scale);
        }
    }

    protected override void OnSelectionChanged(bool iIsSelected)
    {
        _onSelectionChanged(iIsSelected);

        base.OnSelectionChanged(iIsSelected);
    }

    private struct PreviewRectangle
    {
        public PreviewRectangle(Vector2 iPosition, Texture2D iTexture)
        {
            Position = iPosition;
            Texture = iTexture;
        }

        public Vector2 Position { get; }
        public Texture2D Texture { get; }
    }

    private Texture2D _image;
    private readonly Action<bool> _onSelectionChanged;
    
    private Vector2 _topLeft;
    private float _scale;

    private Point? _lastMousePosition;
    private int? _lastScrollWheelValue;

    private readonly List<PreviewRectangle> _previews;
}