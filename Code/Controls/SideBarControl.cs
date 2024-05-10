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

        var spacingInterval = new Vector2(0, Settings.Layout.SideBar.VerticalSpacing);

        var cellWidthTopLeft = Settings.Layout.SideBar.LabelTopLeft;
        var cellWidthRect = new Rectangle(
            Settings.Layout.SideBar.TextBoxTopLeft.ToPoint(),
            Settings.Layout.SideBar.TextBoxSize.ToPoint());

        _cellWidth = 0;
        _cellWidthLabel = new LabelControl(cellWidthTopLeft, "Cell Width", Settings.Layout.SideBar.FontScaling);
        _cellWidthTextBox = new NumericTextBox(cellWidthRect, false, OnCellWidthChanged, Settings.Layout.SideBar.FontScaling);

        var cellHeightTopLeft = Settings.Layout.SideBar.LabelTopLeft + spacingInterval;
        var cellHeightRect = new Rectangle(
            cellWidthRect.Location + spacingInterval.ToPoint(),
            Settings.Layout.SideBar.TextBoxSize.ToPoint());
        _cellHeight = 0;
        _cellHeightLabel = new LabelControl(cellHeightTopLeft, "Cell Height", Settings.Layout.SideBar.FontScaling);
        _cellHeightTextBox = new NumericTextBox(cellHeightRect, false, OnCellHeightChanged, Settings.Layout.SideBar.FontScaling);

        var numCellsXTopLeft = Settings.Layout.SideBar.LabelTopLeft + spacingInterval * 2;
        var numCellsXRect = new Rectangle(
            cellWidthRect.Location + (spacingInterval * 2).ToPoint(),
            Settings.Layout.SideBar.TextBoxSize.ToPoint());
        _numCellsX = 0;
        _numCellsXLabel = new LabelControl(numCellsXTopLeft, "Num Cells X", Settings.Layout.SideBar.FontScaling);
        _NumCellsXTextBox = new NumericTextBox(numCellsXRect, false, OnNumCellsXChanged, Settings.Layout.SideBar.FontScaling);
    }

    public void Update(GameTime iGameTime)
    {
        _cellWidthLabel.Update(iGameTime);
        _cellWidthTextBox.Update(iGameTime);

        _cellHeightLabel.Update(iGameTime);
        _cellHeightTextBox.Update(iGameTime);

        _numCellsXLabel.Update(iGameTime);
        _NumCellsXTextBox.Update(iGameTime);
    }

    public void Draw()
    {
        _cellWidthLabel.Draw();
        _cellWidthTextBox.Draw();

        _cellHeightLabel.Draw();
        _cellHeightTextBox.Draw();

        _numCellsXLabel.Draw();
        _NumCellsXTextBox.Draw();
    }

    private readonly Action<GridConfiguration> _onNewGridConfiguration;

    private int _cellWidth;
    private readonly IGameElement _cellWidthLabel;
    private readonly IGameElement _cellWidthTextBox;

    private int _cellHeight;
    private readonly IGameElement _cellHeightLabel;
    private readonly IGameElement _cellHeightTextBox;

    private int _numCellsX;
    private readonly IGameElement _numCellsXLabel;
    private readonly IGameElement _NumCellsXTextBox;

    private void OnCellWidthChanged(int iNewVal)
    {
        _cellWidth = iNewVal;
        PublishGridConfiguration();
    }

    private void OnCellHeightChanged(int iNewVal)
    {
        _cellHeight = iNewVal;
        PublishGridConfiguration();
    }

    private void OnNumCellsXChanged(int iNewVal)
    {
        _numCellsX = iNewVal;
        PublishGridConfiguration();
    }

    private void PublishGridConfiguration()
    {
        var newConfig = new GridConfiguration(
            _cellWidth,
            _cellHeight,
            _numCellsX, 
            0, 
            0, 
            0, 
            0,
            0);

        _onNewGridConfiguration(newConfig);
    }
}