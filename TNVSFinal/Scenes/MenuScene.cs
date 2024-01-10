using TNVSFinal.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace TNVSFinal.Scenes
{
    public class MenuScene : IScene
    {
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;
        private SpriteBatch _spriteBatch;

        private Texture2D _logoTexture;
        private Texture2D _birdTexture;
        private Texture2D _startButtonTexture;
        private Texture2D _themesButtonTexture;
        private Texture2D _helpButtonTexture;
        private Texture2D _aboutButtonTexture;
        private Texture2D _backgroundTexture;
        private Texture2D _baseTexture;
        private float baseScale;

        private Vector2 _logoPosition;
        private Vector2 _birdPosition;
        private Vector2 _startButtonPosition;
        private Vector2 _themesButtonPosition;
        private Vector2 _helpButtonPosition;
        private Vector2 _aboutButtonPosition;
        private Vector2 _basePosition;

        private float _buttonSpacing = 10f;
        private Song _menuSong;

        private SceneManager _sceneManager;

        public MenuScene(GraphicsDeviceManager graphics, ContentManager content, SceneManager sceneManager)
        {
            _graphics = graphics;
            _content = content;
            _sceneManager = sceneManager;
        }

        public void Initialize()
        {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);


        }

        public void LoadContent()
        {
            _logoTexture = _content.Load<Texture2D>("flappybird-logo");
            _birdTexture = _content.Load<Texture2D>("yellowbird-midflap");
            _startButtonTexture = _content.Load<Texture2D>("btnStart");
            _themesButtonTexture = _content.Load<Texture2D>("btnThemes");
            _helpButtonTexture = _content.Load<Texture2D>("btnHelp");
            _aboutButtonTexture = _content.Load<Texture2D>("btnAbout");
            _backgroundTexture = _content.Load<Texture2D>("background-day");
            _baseTexture = _content.Load<Texture2D>("base");

            _menuSong = _content.Load<Song>("menuSong");

            float logoCenter = (_graphics.PreferredBackBufferWidth - _logoTexture.Width) / 2;
            _logoPosition = new Vector2(logoCenter, 20);
            _birdPosition = new Vector2(logoCenter + _logoTexture.Width + 10, _logoPosition.Y + _logoTexture.Height / 2 - _birdTexture.Height / 2);

            float buttonCenterX = (_graphics.PreferredBackBufferWidth - _startButtonTexture.Width) / 2;
            _startButtonPosition = new Vector2(buttonCenterX, _graphics.PreferredBackBufferHeight / 2 - 80);
            _themesButtonPosition = new Vector2(buttonCenterX, _startButtonPosition.Y + _startButtonTexture.Height + _buttonSpacing);
            _helpButtonPosition = new Vector2(buttonCenterX, _themesButtonPosition.Y + _themesButtonTexture.Height + _buttonSpacing);
            _aboutButtonPosition = new Vector2(buttonCenterX, _helpButtonPosition.Y + _helpButtonTexture.Height + _buttonSpacing);

            _basePosition = new Vector2(0, _graphics.PreferredBackBufferHeight - 112);
            baseScale = (_graphics.PreferredBackBufferHeight * 0.125f) / _baseTexture.Height;
        }

        public void Update(GameTime gameTime)
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(_menuSong);
            }

            MouseState mouseState = Mouse.GetState();
            Point mousePosition = mouseState.Position;

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (new Rectangle(_startButtonPosition.ToPoint(), new Point(_startButtonTexture.Width, _startButtonTexture.Height)).Contains(mousePosition))
                {
                    MediaPlayer.Stop();
                    _sceneManager.ChangeScene("GameScene");
                }
                else if (new Rectangle(_themesButtonPosition.ToPoint(), new Point(_themesButtonTexture.Width, _themesButtonTexture.Height)).Contains(mousePosition))
                {
                    _sceneManager.ChangeScene("ThemeScene");
                }
                else if (new Rectangle(_helpButtonPosition.ToPoint(), new Point(_helpButtonTexture.Width, _helpButtonTexture.Height)).Contains(mousePosition))
                {
                    _sceneManager.ChangeScene("HelpScene");
                }
                else if (new Rectangle(_aboutButtonPosition.ToPoint(), new Point(_aboutButtonTexture.Width, _aboutButtonTexture.Height)).Contains(mousePosition))
                {
                    _sceneManager.ChangeScene("AboutScene");
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.Draw(_baseTexture, new Rectangle(0, _graphics.PreferredBackBufferHeight - (int)(_baseTexture.Height * baseScale), _graphics.PreferredBackBufferWidth, (int)(_baseTexture.Height * baseScale)), Color.White);
            spriteBatch.Draw(_logoTexture, _logoPosition, Color.White);
            spriteBatch.Draw(_birdTexture, _birdPosition, Color.White);
            spriteBatch.Draw(_startButtonTexture, _startButtonPosition, Color.White);
            spriteBatch.Draw(_themesButtonTexture, _themesButtonPosition, Color.White);
            spriteBatch.Draw(_helpButtonTexture, _helpButtonPosition, Color.White);
            spriteBatch.Draw(_aboutButtonTexture, _aboutButtonPosition, Color.White);
            spriteBatch.End();
        }
    }
}
