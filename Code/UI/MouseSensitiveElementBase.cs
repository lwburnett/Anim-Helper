using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Anim_Helper.UI;

internal abstract class MouseSensitiveElementBase : IGameElement
{
    public virtual void Update(GameTime iGameTime)
    {
        if (IsOverlappingWithMouse(Mouse.GetState().Position))
        {
            OnOverlap(iGameTime);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                OnPressed(iGameTime);
            else if (Mouse.GetState().LeftButton == ButtonState.Released)
                OnReleased(iGameTime);
        }
        else
        {
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                Reset();
            }
            else if (_isOverlapped)
            {
                OnNotOverlap(iGameTime);
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

    protected abstract Rectangle HitBox { get; set; }

    protected virtual void OnOverlapBegin(GameTime iGameTime) { }

    protected virtual void OnOverlapEnd(GameTime iGameTime) { }

    protected virtual void OnPress(GameTime iGameTime) { }

    protected virtual void OnRelease(GameTime iGameTime) { }
    
    protected enum PressState
    {
        Default,
        Hover,
        Pressed
    }

    protected PressState StateOfPress;
    private bool _isPressed;
    private bool _isOverlapped;

    protected bool IsOverlappingWithMouse(Point iPosition)
    {
        return iPosition.X > HitBox.X &&
               iPosition.X < HitBox.X + HitBox.Width &&
               iPosition.Y > HitBox.Y &&
               iPosition.Y < HitBox.Y + HitBox.Height;
    }

    private void OnPressed(GameTime iGameTime)
    {
        _isPressed = true;

        OnPress(iGameTime);
    }

    private void OnReleased(GameTime iGameTime)
    {
        if (_isPressed && _isOverlapped)
        {
            OnRelease(iGameTime);
        }

        _isPressed = false;
    }

    private void Reset()
    {
        _isPressed = false;
        _isOverlapped = false;
    }

    private void OnOverlap(GameTime iGameTime)
    {
        _isOverlapped = true;

        OnOverlapBegin(iGameTime);
    }

    private void OnNotOverlap(GameTime iGameTime)
    {
        _isOverlapped = false;

        OnOverlapEnd(iGameTime);
    }
}