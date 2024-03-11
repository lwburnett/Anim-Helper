using Microsoft.Xna.Framework;

namespace Anim_Helper.UI
{
    internal interface IGameElement
    {
        void Update(GameTime iGameTime);
        void Draw();
    }
}
