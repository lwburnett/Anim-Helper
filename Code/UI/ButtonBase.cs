using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Anim_Helper.UI;

public abstract class UiButtonBase : IGameElement
{
    public virtual void Update(GameTime iGameTime)
    {
        if (IsOverlappingWithMouse(Mouse.GetState().Position))
        {
            OnOverlap();

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                OnPressed();
            else if (Mouse.GetState().LeftButton == ButtonState.Released)
                OnReleased(iGameTime);
        }
        else
        {
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                Reset();
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                OnNotOverlap();
            }
        }

        if (!_isOverlapped && !_isPressed)
        {
            StateOfPress = PressState.Default;
        }
        else if (_isOverlapped && !_isPressed)
        {
            StateOfPress = PressState.Hover;
        }
        else
        {
            StateOfPress = PressState.Pressed;
        }
    }

    public abstract void Draw();

    protected abstract Rectangle Bounds { get; set; }
    protected abstract Action<GameTime> OnClickedCallback { get; set; }

    protected enum PressState
    {
        Default,
        Hover,
        Pressed
    }

    protected PressState StateOfPress;
    private bool _isPressed;
    private bool _isOverlapped;

    private bool IsOverlappingWithMouse(Point iPosition)
    {
        return iPosition.X > Bounds.X &&
               iPosition.X < Bounds.X + Bounds.Width &&
               iPosition.Y > Bounds.Y &&
               iPosition.Y < Bounds.Y + Bounds.Height;
    }

    private void OnPressed()
    {
        _isPressed = true;
    }

    private void OnReleased(GameTime iGameTime)
    {
        if (_isPressed && _isOverlapped)
        {
            OnClickedCallback(iGameTime);
        }

        _isPressed = false;
    }

    private void Reset()
    {
        _isPressed = false;
        _isOverlapped = false;
    }

    private void OnOverlap()
    {
        _isOverlapped = true;
    }

    private void OnNotOverlap()
    {
        _isOverlapped = false;
    }
}