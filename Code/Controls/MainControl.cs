using Anim_Helper.UI;
using Microsoft.Xna.Framework;

namespace Anim_Helper.Controls;

internal class MainControl : IGameElement
{
    public MainControl()
    {
        _ribbon = new RibbonControl();
    }

    public void Update(GameTime iGameTime)
    {
        _ribbon.Update(iGameTime);
    }

    public void Draw()
    {
        _ribbon.Draw();
    }

    private RibbonControl _ribbon;
}