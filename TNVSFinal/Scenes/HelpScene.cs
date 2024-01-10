using TNVSFinal.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Input;

namespace TNVSFinal.Scenes
{
    public class HelpScene : IScene
    {
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;
        private SpriteBatch _spriteBatch;

        private Texture2D _backgroundTexture;
        private Texture2D _baseTexture;
        private Texture2D _logoTexture;
        private Texture2D _birdTexture;
        private Texture2D _menuButtonTexture;
        private Texture2D _pipeTexture;
        private Texture2D _pipeInvertedTexture;
        private SpriteFont _font;
        private float baseScale;

        private Vector2 _logoPosition;
        private Vector2 _birdPosition;
        private Vector2 _menuButtonPosition;
        private Vector2 _basePosition;
        private Vector2 _pipePosition;
        private Vector2 _pipeInvertedPosition;
        private Vector2 _textPosition;

        private SceneManager _sceneManager;
        private MouseState _previousMouseState;

        public HelpScene(GraphicsDeviceManager graphics, ContentManager content, SceneManager sceneManager)
        {
            _graphics = graphics;
            _content = content;
            _sceneManager = sceneManager;
        }

        public void Initialize()
        {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            _previousMouseState = Mouse.GetState();
        }

        public void LoadContent()
        {
            _backgroundTexture = _content.Load<Texture2D>("background-day");
            _baseTexture = _content.Load<Texture2D>("base");
            _logoTexture = _content.Load<Texture2D>("flappybird-logo");
            _birdTexture = _content.Load<Texture2D>("yellowbird-midflap");
            _menuButtonTexture = _content.Load<Texture2D>("btnMenu");
            _pipeTexture = _content.Load<Texture2D>("pipe-green");
            _font = _content.Load<SpriteFont>("HelpFont");

            float logoCenter = (_graphics.PreferredBackBufferWidth - _logoTexture.Width) / 2;
            _logoPosition = new Vector2(logoCenter, 20);
            _birdPosition = new Vector2(logoCenter + _logoTexture.Width + 10, _logoPosition.Y + _logoTexture.Height / 2 - _birdTexture.Height / 2);
            _menuButtonPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - 100, _graphics.PreferredBackBufferHeight - 130);

            float pipeVerticalPosition = -300;
            _pipePosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 + 260, pipeVerticalPosition + _pipeTexture.Height + 100);
            _pipeInvertedPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 + 260, pipeVerticalPosition);
            _textPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2 - 50);

            _basePosition = new Vector2(0, _graphics.PreferredBackBufferHeight - 112);
            baseScale = (_graphics.PreferredBackBufferHeight * 0.125f) / _baseTexture.Height;
        }

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();
            Point mousePosition = currentMouseState.Position;

            Rectangle menuButtonRect = new Rectangle(
                (int)_menuButtonPosition.X,
                (int)_menuButtonPosition.Y,
                _menuButtonTexture.Width,
                _menuButtonTexture.Height
            );

            if (_previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed && menuButtonRect.Contains(mousePosition))
            {
                _sceneManager.ChangeScene("MenuScene");
            }

            _previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.Draw(_logoTexture, _logoPosition, Color.White);
            spriteBatch.Draw(_birdTexture, _birdPosition, Color.White);
            spriteBatch.Draw(_menuButtonTexture, _menuButtonPosition, Color.White);
            spriteBatch.Draw(_pipeTexture, _pipePosition, Color.White);
            spriteBatch.Draw(_pipeTexture, _pipeInvertedPosition, null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipVertically, 0f);

            string helpText = "Tap the screen to create a bird. Try not to touch the pipe.";

            string[] helpLines = helpText.Split('\n');
            Vector2 textPosition = _textPosition;

            foreach (string line in helpLines)
            {
                Vector2 textSize = _font.MeasureString(line);
                Vector2 textStartPos = new Vector2(
                    textPosition.X - textSize.X / 2,
                    textPosition.Y - textSize.Y / 2
                );

                spriteBatch.DrawString(_font, line, textStartPos, Color.Black);
                textPosition.Y += textSize.Y; // Move to the next line
            }

            spriteBatch.Draw(_baseTexture, new Rectangle(0, _graphics.PreferredBackBufferHeight - (int)(_baseTexture.Height * baseScale), _graphics.PreferredBackBufferWidth, (int)(_baseTexture.Height * baseScale)), Color.White);

            spriteBatch.End();
        }
    }
}
