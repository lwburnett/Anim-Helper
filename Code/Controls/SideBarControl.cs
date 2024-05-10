using System;
using System.Collections.Generic;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Utils;

namespace Anim_Helper.Controls;

internal class SideBarControl : IGameElement
{
    public SideBarControl(Action<GridConfiguration> iOnNewGridConfiguration)
    {
        _onNewGridConfiguration = iOnNewGridConfiguration;

        var textBoxRect = new Rectangle(
            Settings.Layout.SideBar.TextBoxTopLeft.ToPoint(),
            Settings.Layout.SideBar.TextBoxSize.ToPoint());

        _numX = 0;
        _numXLabel = new LabelControl(Settings.Layout.SideBar.LabelTopLeft, "Num X", Settings.Layout.SideBar.FontScaling);
        _numXTextBox = new NumericTextBox(textBoxRect, false, OnNumXChanged, Settings.Layout.SideBar.FontScaling);
    }

    public void Update(GameTime iGameTime)
    {
        _numXLabel.Update(iGameTime);
        _numXTextBox.Update(iGameTime);
    }

    public void Draw()
    {
        _numXLabel.Draw();
        _numXTextBox.Draw();
    }

    private readonly Action<GridConfiguration> _onNewGridConfiguration;

    private int _numX;
    private readonly IGameElement _numXLabel;
    private readonly IGameElement _numXTextBox;

    private void OnNumXChanged(int iNewVal)
    {
        _numX = iNewVal;
        PublishGridConfiguration();
    }

    private void PublishGridConfiguration()
    {
        var newConfig = new GridConfiguration(
            _numX, 
            0, 
            0, 
            0, 
            0, 
            0, 
            new List<int>{0, 0, 0, 0});

        _onNewGridConfiguration(newConfig);
    }
}