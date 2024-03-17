using System.Collections.Generic;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Anim_Helper.Controls;

internal class MainControl : IGameElement
{
    public MainControl()
    {
        _ribbon = new RibbonControl(OnNewSprites);
        _flipbook = new FlipbookControl();
    }

    public void Update(GameTime iGameTime)
    {
        _ribbon.Update(iGameTime);
        _flipbook.Update(iGameTime);
    }

    public void Draw()
    {
        _ribbon.Draw();
        _flipbook.Draw();
    }

    private readonly RibbonControl _ribbon;
    private readonly FlipbookControl _flipbook;

    private void OnNewSprites(List<Sprite2D> iSprites)
    {
        _flipbook.SetSprites(iSprites);
    }
}