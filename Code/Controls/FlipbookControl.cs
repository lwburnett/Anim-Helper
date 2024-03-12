using System.Collections.Generic;
using System.Linq;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Anim_Helper.Controls;

internal class FlipbookControl : SelectableElementBase
{
    public FlipbookControl()
    {
        HitBox = new Rectangle();
        _currentSpriteIndex = -1;
        _currentSpriteTime = 0;
        _sprites = new List<Texture2D>();
        _fps = 2;
        _topLeft = new Vector2(400);
        _lastMousePosition = null;
    }

    public override void Update(GameTime iGameTime)
    {
        if (_currentSpriteIndex >= 0)
            _currentSpriteTime += (float)iGameTime.ElapsedGameTime.TotalSeconds;

        if (_currentSpriteTime >= 1.0f / _fps)
        {
            _currentSpriteIndex = _currentSpriteIndex < _sprites.Count - 1 ? _currentSpriteIndex + 1 : 0;
            _currentSpriteTime = 0;
        }

        var mouseState = Mouse.GetState();
        if (IsSelected && IsOverlappingWithMouse(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
        {
            if (_lastMousePosition.HasValue)
            {
                var mouseDiff = mouseState.Position - _lastMousePosition.Value;
                _topLeft += mouseDiff.ToVector2();
                HitBox = new Rectangle(_topLeft.ToPoint(), new Point(HitBox.Width, HitBox.Height));
            }

            _lastMousePosition = mouseState.Position;
        }

        if (IsSelected && mouseState.LeftButton == ButtonState.Released)
            _lastMousePosition = null;

        base.Update(iGameTime);
    }
    
    protected override void vDraw()
    {
        if (_currentSpriteIndex >= 0)
        {
            GraphicsHelper.DrawTexture(_sprites[_currentSpriteIndex], _topLeft);
        }
    }

    public void SetSprites(List<Texture2D> iNewSprites)
    {
        _currentSpriteIndex = iNewSprites.Any() ? 0 : -1;
        _currentSpriteTime = 0;
        _sprites = iNewSprites;

        var width = iNewSprites.Max(s => s.Width);
        var height = iNewSprites.Max(s => s.Height);

        HitBox = new Rectangle(_topLeft.ToPoint(), new Point(width, height));
    }

    private int _currentSpriteIndex;
    private float _currentSpriteTime;
    private List<Texture2D> _sprites;
    private int _fps;
    private Vector2 _topLeft;

    private Point? _lastMousePosition;
}