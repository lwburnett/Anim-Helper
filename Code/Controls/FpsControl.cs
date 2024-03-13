using System;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;

namespace Anim_Helper.Controls;

internal class FpsControl : IGameElement
{
    public FpsControl(Vector2 iTopLeft, int iInitialFps, Action<int> iOnNewFpsAction)
    {
        _topLeft = iTopLeft;
        _fps = iInitialFps;
        _onNewFpsAction = iOnNewFpsAction;

        var leftBounds = new Rectangle(iTopLeft.ToPoint(), Settings.Layout.Ribbon.FramePreview.ButtonSize.ToPoint());
        _leftButton = new TextButton(leftBounds, "-", _ => OnButtonClicked(false));

        var rightBounds =
            new Rectangle((iTopLeft + new Vector2(Settings.Layout.Flipbook.FpsButtonSpacing, 0)).ToPoint(),
                Settings.Layout.Ribbon.FramePreview.ButtonSize.ToPoint());
        _rightButton = new TextButton(rightBounds, "+", _ => OnButtonClicked(true));
    }

    public void Update(GameTime iGameTime)
    {
        _leftButton.Update(iGameTime);
        _rightButton.Update(iGameTime);
    }

    public void Draw()
    {
        _leftButton.Draw();
        _rightButton.Draw();

        var text = _fps.ToString();
        var stringDimensions = GraphicsHelper.GetFont().MeasureString(text);
        var center = _topLeft + 
                     new Vector2((Settings.Layout.Ribbon.FramePreview.ButtonSize.X + Settings.Layout.Flipbook.FpsButtonSpacing) / 2,
                         Settings.Layout.Ribbon.FramePreview.ButtonSize.Y / 2);
        GraphicsHelper.DrawString(text, center - stringDimensions / 2, Color.Black);
    }

    public void Move(Vector2 iNewTopLeft)
    {
        _topLeft = iNewTopLeft;
        _leftButton.Move(iNewTopLeft);
        _rightButton.Move(iNewTopLeft + new Vector2(Settings.Layout.Flipbook.FpsButtonSpacing, 0));
    }
    private Vector2 _topLeft;
    private int _fps;
    private readonly Action<int> _onNewFpsAction;
    private readonly TextButton _leftButton;
    private readonly TextButton _rightButton;

    private void OnButtonClicked(bool iRight)
    {
        if ((iRight && _fps == Settings.MaxFps) || (!iRight && _fps == 1))
            return;

        _fps += iRight ? 1 : -1;

        _onNewFpsAction(_fps);
    }
}