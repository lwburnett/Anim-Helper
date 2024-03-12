using Microsoft.Xna.Framework;
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

    protected override void OnRelease(GameTime iGameTime)
    {
        IsSelected = true;

        base.OnRelease(iGameTime);
    }

    protected bool IsSelected { get; private set; }
}