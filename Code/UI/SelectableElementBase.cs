using System;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Anim_Helper.UI;

internal abstract class SelectableElementBase : MouseSensitiveElementBase
{
    public override void Update(GameTime iGameTime)
    {
        var mouseState = Mouse.GetState();
        if (IsSelected && mouseState.LeftButton == ButtonState.Pressed && !IsOverlappingWithMouse(mouseState.Position))
        {
            IsSelected = false;
        }

        base.Update(iGameTime);
    }

    public sealed override void Draw()
    {
        if (HitBox.IsEmpty)
            return;

        if (IsSelected)
            GraphicsHelper.DrawTexture(SelectTexture, HitBox.Location.ToVector2(), _selectScale);

        vDraw();
    }

    protected sealed override Rectangle HitBox
    {
        get => _bHitBox;
        set
        {
            if (value.IsEmpty)
            {
                _bHitBox = value;
                return;
            }

            var newRatio = (float)value.Height / value.Width; 
            var oldRatio = _bHitBox.Width > 0 ?(float)_bHitBox.Height / _bHitBox.Width : 0.0f; 

            if (_bHitBox.IsEmpty || Math.Abs(newRatio - oldRatio) > .01 )
            {
                var backgroundDataSize = value.Width * value.Height;
                var backgroundColorData = new Color[backgroundDataSize];

                for (var ii = 0; ii < backgroundDataSize; ii++)
                {
                    backgroundColorData[ii] = Settings.Colors.HoverBackground;
                }

                SelectTexture = GraphicsHelper.CreateTexture(backgroundColorData, value.Width, value.Height);
                _selectScale = 1.0f;
            }
            else if (Math.Abs(newRatio - oldRatio) <= .01)
            {
                _selectScale *= (float)value.Height / _bHitBox.Height;
            }

            _bHitBox = value;
        }
    }
    private Rectangle _bHitBox = Rectangle.Empty;

    private float _selectScale = 1.0f;

    protected Texture2D SelectTexture { get; private set; }

    protected override void OnPress(GameTime iGameTime)
    {
        IsSelected = true;

        base.OnPress(iGameTime);
    }

    protected bool IsSelected
    {
        get; private set;
    }

    protected abstract void vDraw();
}