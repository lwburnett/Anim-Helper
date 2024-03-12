using System;
using Microsoft.Xna.Framework;

namespace Anim_Helper.UI;

internal abstract class UiButtonBase : MouseSensitiveElementBase
{
    protected abstract Action<GameTime> OnClickedCallback { get; set; }

    protected override void OnRelease(GameTime iGameTime)
    {
        OnClickedCallback(iGameTime);
    }
}