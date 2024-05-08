using System.Collections.Generic;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;

namespace Anim_Helper.Controls;

internal class MainControl : IGameElement
{
    public MainControl()
    {
        _ribbon = new RibbonControl(OnNewSprites, OnParserTypeChanged);
        _flipbook = new FlipbookControl();
        _sideBar = new SideBarControl();
        _isSideBarVisible = false;
    }

    public void Update(GameTime iGameTime)
    {
        _ribbon.Update(iGameTime);
        _flipbook.Update(iGameTime);

        if (_isSideBarVisible)
            _sideBar.Update(iGameTime);
    }

    public void Draw()
    {
        _ribbon.Draw();
        _flipbook.Draw();

        if (_isSideBarVisible)
            _sideBar.Draw();
    }

    private readonly RibbonControl _ribbon;
    private readonly FlipbookControl _flipbook;
    private readonly SideBarControl _sideBar;
    private bool _isSideBarVisible;

    private void OnNewSprites(List<Sprite2D> iSprites)
    {
        _flipbook.SetSprites(iSprites);
    }

    private void OnParserTypeChanged(bool iIsGrid)
    {
        _isSideBarVisible = iIsGrid;
    }
}