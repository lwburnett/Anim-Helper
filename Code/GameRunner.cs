using System;
using System.Collections.Generic;
using Anim_Helper.Controls;
using Anim_Helper.UI;
using Anim_Helper.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Anim_Helper;

public class GameRunner : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private readonly List<IGameElement> _gameElements;

    public GameRunner()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnResize;

        _gameElements = new List<IGameElement>();
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1000;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        GraphicsHelper.RegisterContentManager(Content);
        GraphicsHelper.RegisterGraphicsDevice(GraphicsDevice);
        GraphicsHelper.RegisterSpriteBatch(_spriteBatch);

        _gameElements.Add(new MainControl());
    }

    protected override void Update(GameTime iGameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        foreach (var gameElement in _gameElements)
        {
            gameElement.Update(iGameTime);
        }

        base.Update(iGameTime);
    }

    protected override void Draw(GameTime iGameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

        foreach (var gameElement in _gameElements)
        {
            gameElement.Draw();
        }
        
        _spriteBatch.End();

        base.Draw(iGameTime);
    }

    private void OnResize(object iSender, EventArgs iEventArgs)
    {
        var gameWindow = (GameWindow)iSender;

        if (gameWindow == null)
            return;

        _graphics.PreferredBackBufferWidth = gameWindow.ClientBounds.Width;
        _graphics.PreferredBackBufferHeight = gameWindow.ClientBounds.Height;
        _graphics.ApplyChanges();
    }
}
