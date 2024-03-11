using System;
using System.Collections.Generic;
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

        GraphicsHelper.RegisterContentManager(Content);
        GraphicsHelper.RegisterGraphicsDevice(GraphicsDevice);
        GraphicsHelper.RegisterSpriteBatch(_spriteBatch);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        foreach (var gameElement in _gameElements)
        {
            gameElement.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        foreach (var gameElement in _gameElements)
        {
            gameElement.Draw();
        }

        base.Draw(gameTime);
    }

    private void OnResize(object sender, EventArgs e)
    {
        var gameWindow = (GameWindow)sender;

        if (gameWindow == null)
            return;

        _graphics.PreferredBackBufferWidth = gameWindow.ClientBounds.Width;
        _graphics.PreferredBackBufferHeight = gameWindow.ClientBounds.Height;
        _graphics.ApplyChanges();
    }
}
