using System;
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

        _cellWidth = 0;
        _cellWidthLabel = new LabelControl(Settings.Layout.SideBar.LabelTopLeft, "Cell Width", Settings.Layout.SideBar.FontScaling);
        _cellWidthTextBox = new NumericTextBox(textBoxRect, false, OnCellWidthChanged, Settings.Layout.SideBar.FontScaling);
    }

    public void Update(GameTime iGameTime)
    {
        _cellWidthLabel.Update(iGameTime);
        _cellWidthTextBox.Update(iGameTime);
    }

    public void Draw()
    {
        _cellWidthLabel.Draw();
        _cellWidthTextBox.Draw();
    }

    private readonly Action<GridConfiguration> _onNewGridConfiguration;

    private int _cellWidth;
    private readonly IGameElement _cellWidthLabel;
    private readonly IGameElement _cellWidthTextBox;

    private void OnCellWidthChanged(int iNewVal)
    {
        _cellWidth = iNewVal;
        PublishGridConfiguration();
    }

    private void PublishGridConfiguration()
    {
        var newConfig = new GridConfiguration(
            _cellWidth, 
            0, 
            0, 
            0, 
            0, 
            0, 
            0,
            0);

        _onNewGridConfiguration(newConfig);
    }
}