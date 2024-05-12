using System;
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
    }

    public void SetImage(Texture2D iImage)
    {
        if (iImage == null)
            return;

        _image = iImage;

        HitBox = new Rectangle(_topLeft.ToPoint(), new Point(_image.Width, _image.Height));
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
    }

    protected override void OnSelectionChanged(bool iIsSelected)
    {
        _onSelectionChanged(iIsSelected);

        base.OnSelectionChanged(iIsSelected);
    }

    private Texture2D _image;
    private readonly Action<bool> _onSelectionChanged;
    
    private Vector2 _topLeft;
    private float _scale;

    private Point? _lastMousePosition;
    private int? _lastScrollWheelValue;
}