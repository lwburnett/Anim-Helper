using System.Collections.Generic;
using System.Linq;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Anim_Helper.Controls;

internal class FlipbookControl : IGameElement
{
    public FlipbookControl()
    {
        _currentSpriteIndex = -1;
        _currentSpriteTime = 0;
        _sprites = new List<Texture2D>();
        _fps = 2;
    }

    public void Update(GameTime iGameTime)
    {
        if (_currentSpriteIndex >= 0)
            _currentSpriteTime += (float)iGameTime.ElapsedGameTime.TotalSeconds;

        if (_currentSpriteTime >= 1.0f / _fps)
        {
            _currentSpriteIndex = _currentSpriteIndex < _sprites.Count - 1 ? _currentSpriteIndex + 1 : 0;
            _currentSpriteTime = 0;
        }
    }

    public void Draw()
    {
        if (_currentSpriteIndex >= 0)
        {
            GraphicsHelper.DrawTexture(_sprites[_currentSpriteIndex], new Vector2(400, 400));
        }
    }

    public void SetSprites(List<Texture2D> iNewSprites)
    {
        _currentSpriteIndex = iNewSprites.Any() ? 0 : -1;
        _currentSpriteTime = 0;
        _sprites = iNewSprites;
    }

    private int _currentSpriteIndex;
    private float _currentSpriteTime;
    private List<Texture2D> _sprites;
    private int _fps;
}