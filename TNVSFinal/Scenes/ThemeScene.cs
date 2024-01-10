using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TNVSFinal.Scenes;
using Microsoft.Xna.Framework.Content;

namespace TNVSFinal.Scenes
{
    public class ThemeScene : IScene
    {
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private SceneManager _sceneManager;
        private Texture2D _backgroundTexture;
        private Texture2D _baseTexture;
        private Texture2D _logoTexture;
        private Texture2D _birdTexture;
        private Texture2D _themesTexture;
        private Texture2D _dayTexture;
        private Texture2D _nightTexture;
        private Texture2D _menuButtonTexture;
        private float baseScale;

        private Rectangle _menuButtonHitbox;

        private Vector2 _logoPosition;
        private Vector2 _birdPosition;
        private Vector2 _themesPosition;
        private Vector2 _themeBirdsPosition;
        private Vector2 _themeInkPosition;
        private Vector2 _menuButtonPosition;
        private Vector2 _basePosition;

        private float _buttonSpacing = 10f;

        public ThemeScene(GraphicsDeviceManager graphics, ContentManager content, SceneManager sceneManager)
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
            _backgroundTexture = _content.Load<Texture2D>("background-day");
            _baseTexture = _content.Load<Texture2D>("base");
            _logoTexture = _content.Load<Texture2D>("flappybird-logo");
            _birdTexture = _content.Load<Texture2D>("yellowbird-midflap");
            _themesTexture = _content.Load<Texture2D>("btnBigTheme");
            _dayTexture = _content.Load<Texture2D>("day");
            _nightTexture = _content.Load<Texture2D>("night");
            _menuButtonTexture = _content.Load<Texture2D>("btnMenu");

            float logoCenter = (_graphics.PreferredBackBufferWidth - _logoTexture.Width) / 2;
            _logoPosition = new Vector2(logoCenter, 20);
            _birdPosition = new Vector2(logoCenter + _logoTexture.Width + 10, _logoPosition.Y + _logoTexture.Height / 2 - _birdTexture.Height / 2);

            float buttonCenterX = (_graphics.PreferredBackBufferWidth - _themesTexture.Width) / 2;
            _themesPosition = new Vector2(buttonCenterX, _graphics.PreferredBackBufferHeight / 2 - 80);
            _themeBirdsPosition = new Vector2(buttonCenterX + 15, _themesPosition.Y + _themesTexture.Height + _buttonSpacing);
            _themeInkPosition = new Vector2(buttonCenterX + 15, _themeBirdsPosition.Y + _dayTexture.Height + _buttonSpacing);
            _menuButtonPosition = new Vector2(buttonCenterX, _themeInkPosition.Y + _nightTexture.Height + _buttonSpacing);

            _menuButtonHitbox = new Rectangle((int)_menuButtonPosition.X, (int)_menuButtonPosition.Y, _menuButtonTexture.Width, _menuButtonTexture.Height);

            _basePosition = new Vector2(0, _graphics.PreferredBackBufferHeight - 112);
            baseScale = (_graphics.PreferredBackBufferHeight * 0.125f) / _baseTexture.Height;
        }

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Pressed &&
                _menuButtonHitbox.Contains(currentMouseState.Position))
            {
                _sceneManager.ChangeScene("MenuScene");
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.Draw(_baseTexture, new Rectangle(0, _graphics.PreferredBackBufferHeight - (int)(_baseTexture.Height * baseScale), _graphics.PreferredBackBufferWidth, (int)(_baseTexture.Height * baseScale)), Color.White);
            spriteBatch.Draw(_logoTexture, _logoPosition, Color.White);
            spriteBatch.Draw(_birdTexture, _birdPosition, Color.White);

            spriteBatch.Draw(_themesTexture, _themesPosition, Color.White);
            spriteBatch.Draw(_dayTexture, _themeBirdsPosition, Color.White);
            spriteBatch.Draw(_nightTexture, _themeInkPosition, Color.White);
            spriteBatch.Draw(_menuButtonTexture, _menuButtonPosition, Color.White);

            spriteBatch.End();
        }
    }
}