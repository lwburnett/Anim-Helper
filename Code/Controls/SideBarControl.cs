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

        var numCellsXTopLeft = Settings.Layout.SideBar.LabelTopLeft + spacingInterval * 2.5f;
        var numCellsXRect = new Rectangle(
            cellWidthRect.Location + (spacingInterval * 2.5f).ToPoint(),
            Settings.Layout.SideBar.TextBoxSize.ToPoint());
        _numCellsX = 0;
        _numCellsXLabel = new LabelControl(numCellsXTopLeft, "Num Cells X", Settings.Layout.SideBar.FontScaling);
        _numCellsXTextBox = new NumericTextBox(numCellsXRect, false, OnNumCellsXChanged, Settings.Layout.SideBar.FontScaling);

        var numCellsYTopLeft = Settings.Layout.SideBar.LabelTopLeft + spacingInterval * 3.5f;
        var numCellsYRect = new Rectangle(
            cellWidthRect.Location + (spacingInterval * 3.5f).ToPoint(),
            Settings.Layout.SideBar.TextBoxSize.ToPoint());
        _numCellsY = 0;
        _numCellsYLabel = new LabelControl(numCellsYTopLeft, "Num Cells Y", Settings.Layout.SideBar.FontScaling);
        _numCellsYTextBox = new NumericTextBox(numCellsYRect, false, OnNumCellsYChanged, Settings.Layout.SideBar.FontScaling);

        var marginXTopLeft = Settings.Layout.SideBar.LabelTopLeft + spacingInterval * 5;
        var marginXRect = new Rectangle(
            cellWidthRect.Location + (spacingInterval * 5).ToPoint(),
            Settings.Layout.SideBar.TextBoxSize.ToPoint());
        _marginX = 0;
        _marginXLabel = new LabelControl(marginXTopLeft, "Margin X", Settings.Layout.SideBar.FontScaling);
        _marginXTextBox = new NumericTextBox(marginXRect, false, OnMarginXChanged, Settings.Layout.SideBar.FontScaling);

        var marginYTopLeft = Settings.Layout.SideBar.LabelTopLeft + spacingInterval * 6;
        var marginYRect = new Rectangle(
            cellWidthRect.Location + (spacingInterval * 6).ToPoint(),
            Settings.Layout.SideBar.TextBoxSize.ToPoint());
        _marginY = 0;
        _marginYLabel = new LabelControl(marginYTopLeft, "Margin Y", Settings.Layout.SideBar.FontScaling);
        _marginYTextBox = new NumericTextBox(marginYRect, false, OnMarginYChanged, Settings.Layout.SideBar.FontScaling);

        var spacingXTopLeft = Settings.Layout.SideBar.LabelTopLeft + spacingInterval * 7.5f;
        var spacingXRect = new Rectangle(
            cellWidthRect.Location + (spacingInterval * 7.5f).ToPoint(),
            Settings.Layout.SideBar.TextBoxSize.ToPoint());
        _spacingX = 0;
        _spacingXLabel = new LabelControl(spacingXTopLeft, "Spacing X", Settings.Layout.SideBar.FontScaling);
        _spacingXTextBox = new NumericTextBox(spacingXRect, false, OnSpacingXChanged, Settings.Layout.SideBar.FontScaling);

        var spacingYTopLeft = Settings.Layout.SideBar.LabelTopLeft + spacingInterval * 8.5f;
        var spacingYRect = new Rectangle(
            cellWidthRect.Location + (spacingInterval * 8.5f).ToPoint(),
            Settings.Layout.SideBar.TextBoxSize.ToPoint());
        _spacingY = 0;
        _spacingYLabel = new LabelControl(spacingYTopLeft, "Spacing Y", Settings.Layout.SideBar.FontScaling);
        _spacingYTextBox = new NumericTextBox(spacingYRect, false, OnSpacingYChanged, Settings.Layout.SideBar.FontScaling);
    }

    public void Update(GameTime iGameTime)
    {
        _cellWidthLabel.Update(iGameTime);
        _cellWidthTextBox.Update(iGameTime);

        _cellHeightLabel.Update(iGameTime);
        _cellHeightTextBox.Update(iGameTime);

        _numCellsXLabel.Update(iGameTime);
        _numCellsXTextBox.Update(iGameTime);

        _numCellsYLabel.Update(iGameTime);
        _numCellsYTextBox.Update(iGameTime);

        _marginXLabel.Update(iGameTime);
        _marginXTextBox.Update(iGameTime);

        _marginYLabel.Update(iGameTime);
        _marginYTextBox.Update(iGameTime);

        _spacingXLabel.Update(iGameTime);
        _spacingXTextBox.Update(iGameTime);

        _spacingYLabel.Update(iGameTime);
        _spacingYTextBox.Update(iGameTime);
    }

    public void Draw()
    {
        _cellWidthLabel.Draw();
        _cellWidthTextBox.Draw();

        _cellHeightLabel.Draw();
        _cellHeightTextBox.Draw();

        _numCellsXLabel.Draw();
        _numCellsXTextBox.Draw();

        _numCellsYLabel.Draw();
        _numCellsYTextBox.Draw();

        _marginXLabel.Draw();
        _marginXTextBox.Draw();

        _marginYLabel.Draw();
        _marginYTextBox.Draw();

        _spacingXLabel.Draw();
        _spacingXTextBox.Draw();

        _spacingYLabel.Draw();
        _spacingYTextBox.Draw();
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
    private readonly IGameElement _numCellsXTextBox;

    private int _numCellsY;
    private readonly IGameElement _numCellsYLabel;
    private readonly IGameElement _numCellsYTextBox;

    private int _marginX;
    private readonly IGameElement _marginXLabel;
    private readonly IGameElement _marginXTextBox;

    private int _marginY;
    private readonly IGameElement _marginYLabel;
    private readonly IGameElement _marginYTextBox;

    private int _spacingX;
    private readonly IGameElement _spacingXLabel;
    private readonly IGameElement _spacingXTextBox;

    private int _spacingY;
    private readonly IGameElement _spacingYLabel;
    private readonly IGameElement _spacingYTextBox;

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

    private void OnNumCellsYChanged(int iNewVal)
    {
        _numCellsY = iNewVal;
        PublishGridConfiguration();
    }

    private void OnMarginXChanged(int iNewVal)
    {
        _marginX = iNewVal;
        PublishGridConfiguration();
    }

    private void OnMarginYChanged(int iNewVal)
    {
        _marginY = iNewVal;
        PublishGridConfiguration();
    }

    private void OnSpacingXChanged(int iNewVal)
    {
        _spacingX = iNewVal;
        PublishGridConfiguration();
    }

    private void OnSpacingYChanged(int iNewVal)
    {
        _spacingY = iNewVal;
        PublishGridConfiguration();
    }

    private void PublishGridConfiguration()
    {
        var newConfig = new GridConfiguration(
            _cellWidth,
            _cellHeight,
            _numCellsX,
            _numCellsY, 
            _marginX, 
            _marginY, 
            _spacingX,
            _spacingY);

        _onNewGridConfiguration(newConfig);
    }
}