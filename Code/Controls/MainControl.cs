using System.Collections.Generic;
using Anim_Helper.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Anim_Helper.Controls;

internal class MainControl : IGameElement
{
    public MainControl()
    {
        _ribbon = new RibbonControl(OnNewSprites);
    }

    public void Update(GameTime iGameTime)
    {
        _ribbon.Update(iGameTime);
    }

    public void Draw()
    {
        _ribbon.Draw();
    }

    private readonly RibbonControl _ribbon;

    private void OnNewSprites(List<Texture2D> iSprites)
    {
        //todo
    }
}