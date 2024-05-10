using System.Collections.Generic;
using System.Linq;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Anim_Helper.Controls;

internal class FlipbookControl : SelectableElementBase
{
    public FlipbookControl()
    {
        HitBox = new Rectangle();
        _currentSpriteIndex = -1;
        _currentSpriteTime = 0;
        _frames = new List<Sprite2D>();
        _fpsControl = new FpsControl(Vector2.Zero, Settings.DefaultFps, OnChangeFps);
        _fps = Settings.DefaultFps;
        _topLeft = new Vector2(400);
        _scale = 1.0f;
        _lastMousePosition = null;
        _lastScrollWheelValue = null;
    }

    public override void Update(GameTime iGameTime)
    {
        if (!_frames.Any())
            return;

        // Handle change of frames
        if (_currentSpriteIndex >= 0)
            _currentSpriteTime += (float)iGameTime.ElapsedGameTime.TotalSeconds;

        if (_currentSpriteTime >= 1.0f / _fps)
        {
            _currentSpriteIndex = _currentSpriteIndex < _frames.Count - 1 ? _currentSpriteIndex + 1 : 0;
            _currentSpriteTime = 0;
        }

        // Handle click and drag
        var mouseState = Mouse.GetState();
        if (IsSelected && IsOverlappingWithMouse(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
        {
            if (_lastMousePosition.HasValue)
            {
                var mouseDiff = mouseState.Position - _lastMousePosition.Value;
                _topLeft += mouseDiff.ToVector2();
                HitBox = new Rectangle(_topLeft.ToPoint(), new Point(HitBox.Width, HitBox.Height));
                _fpsControl.Move(HitBox.Location.ToVector2());
            }

            _lastMousePosition = mouseState.Position;
        }

        if (_lastMousePosition.HasValue && ((IsSelected && mouseState.LeftButton == ButtonState.Released) || !IsSelected))
            _lastMousePosition = null;

        // Handle mouse wheel
        if (IsSelected && IsOverlappingWithMouse(mouseState.Position))
        {
            if (_lastScrollWheelValue.HasValue)
            {
                var scrollDiff = _lastScrollWheelValue.Value - mouseState.ScrollWheelValue;

                if (scrollDiff != 0)
                {
                    _scale += scrollDiff * Settings.Layout.Flipbook.ScaleChangePerWheelTick;

                    var width = _frames.Max(s => s.SourceRect.Width);
                    var height = _frames.Max(s => s.SourceRect.Height);
                    HitBox = new Rectangle(HitBox.Location, (new Vector2(width, height) * _scale).ToPoint());
                }
            }

            _lastScrollWheelValue = mouseState.ScrollWheelValue;
        }
        else if (_lastScrollWheelValue.HasValue)
            _lastScrollWheelValue = null;

        // Handle FPS control
        if (IsSelected)
            _fpsControl.Update(iGameTime);

        base.Update(iGameTime);
    }
    
    protected override void vDraw()
    {
        if (_currentSpriteIndex >= 0)
        {
            var sprite = _frames[_currentSpriteIndex];
            GraphicsHelper.DrawTexture(sprite.Texture, _topLeft, sprite.SourceRect, _scale);

            if (IsSelected)
                _fpsControl.Draw();
        }
    }

    public void SetFrames(List<Sprite2D> iNewFrames)
    {
        _currentSpriteIndex = iNewFrames.Any() ? 0 : -1;
        _currentSpriteTime = 0;
        _frames = iNewFrames;

        if (!_frames.Any())
            return;

        var width = iNewFrames.Max(s => s.SourceRect.Width);
        var height = iNewFrames.Max(s => s.SourceRect.Height);

        HitBox = new Rectangle(_topLeft.ToPoint(), new Point(width, height));
        _fpsControl.Move(HitBox.Location.ToVector2());
    }

    private int _currentSpriteIndex;
    private float _currentSpriteTime;
    private List<Sprite2D> _frames;
    private readonly FpsControl _fpsControl;
    private int _fps;
    private Vector2 _topLeft;
    private float _scale;

    private Point? _lastMousePosition;
    private int? _lastScrollWheelValue;

    private void OnChangeFps(int iNewFps) => _fps = iNewFps;
}