using TNVSFinal.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TNVSFinal;

namespace TNVSFinal
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SceneManager _sceneManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _sceneManager = new SceneManager();
        }

        protected override void Initialize()
        {
            _sceneManager.AddScene("MenuScene", new MenuScene(_graphics, Content, _sceneManager));
            _sceneManager.AddScene("GameScene", new GameScene(_graphics, Content, _sceneManager));
            _sceneManager.AddScene("HelpScene", new HelpScene(_graphics, Content, _sceneManager));
            _sceneManager.AddScene("AboutScene", new AboutScene(_graphics, Content, _sceneManager));
            _sceneManager.AddScene("RankingScene", new RankingScene(_graphics, Content, _sceneManager));
            _sceneManager.AddScene("ThemeScene", new ThemeScene(_graphics, Content, _sceneManager));
            _sceneManager.ChangeScene("MenuScene");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _sceneManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _sceneManager.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
