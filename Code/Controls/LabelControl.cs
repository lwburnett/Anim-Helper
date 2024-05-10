using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;

namespace Anim_Helper.Controls;

internal class LabelControl : IGameElement
{
    public LabelControl(Vector2 iTopLeft, string iText)
    {
        _topLeft = iTopLeft;
        _text = iText;
    }

    public void Update(GameTime iGameTime)
    {
    }

    public void Draw()
    {
        GraphicsHelper.DrawString(_text, _topLeft, Color.Black);
    }

    private readonly Vector2 _topLeft;
    private readonly string _text;
}