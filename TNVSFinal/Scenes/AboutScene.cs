using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TNVSFinal.Scenes;
using Microsoft.Xna.Framework.Content;

namespace TNVSFinal.Scenes
{
    public class AboutScene : IScene
    {
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;
        private SpriteBatch _spriteBatch;

        private Texture2D _backgroundTexture;
        private Texture2D _baseTexture;
        private Texture2D _logoTexture;
        private Texture2D _birdTexture;
        private Texture2D _creatorsTexture;
        private Texture2D _player1Texture;
        private Texture2D _player2Texture;
        private Texture2D _menuButtonTexture;
        private Texture2D _player1;
        private Texture2D _player2;

        private float baseScale;
        private MouseState _previousMouseState;

        private Vector2 _logoPosition;
        private Vector2 _birdPosition;
        private Vector2 _creatorsPosition;
        private Vector2 _player1Position;
        private Vector2 _player2Position;
        private Vector2 _menuButtonPosition;
        private Vector2 _basePosition;

        private Rectangle _player1Hitbox;
        private Rectangle _player2Hitbox;
        private Rectangle _menuButtonHitbox;

        private bool _displayPlayer1 = false;
        private bool _displayPlayer2 = false;



        private float _buttonSpacing = 10f;

        private SceneManager _sceneManager;

        public AboutScene(GraphicsDeviceManager graphics, ContentManager content, SceneManager sceneManager)
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
            _creatorsTexture = _content.Load<Texture2D>("creators");
            _player1Texture = _content.Load<Texture2D>("ThiNguyen");
            _player2Texture = _content.Load<Texture2D>("VidhiSavaliya");
            _menuButtonTexture = _content.Load<Texture2D>("btnMenu");
            _player1 = _content.Load<Texture2D>("Player1");
            _player2 = _content.Load<Texture2D>("Player2");


            float logoCenter = (_graphics.PreferredBackBufferWidth - _logoTexture.Width) / 2;
            _logoPosition = new Vector2(logoCenter, 20);
            _birdPosition = new Vector2(logoCenter + _logoTexture.Width + 10, _logoPosition.Y + _logoTexture.Height / 2 - _birdTexture.Height / 2);

            float buttonCenterX = (_graphics.PreferredBackBufferWidth - _creatorsTexture.Width) / 2;
            float playerStartX = buttonCenterX + 15;
            _creatorsPosition = new Vector2(buttonCenterX, _graphics.PreferredBackBufferHeight / 2 - 80);
            _player1Position = new Vector2(playerStartX, _creatorsPosition.Y + _creatorsTexture.Height + _buttonSpacing);
            _player2Position = new Vector2(playerStartX, _player1Position.Y + _player1Texture.Height + _buttonSpacing);
            _menuButtonPosition = new Vector2(buttonCenterX, _player2Position.Y + _player2Texture.Height + _buttonSpacing);

            // Define hitboxes for interactive elements
            _player1Hitbox = new Rectangle(_player1Position.ToPoint(), new Point(_player1Texture.Width, _player1Texture.Height));
            _player2Hitbox = new Rectangle(_player2Position.ToPoint(), new Point(_player2Texture.Width, _player2Texture.Height));
            _menuButtonHitbox = new Rectangle(_menuButtonPosition.ToPoint(), new Point(_menuButtonTexture.Width, _menuButtonTexture.Height));

            _basePosition = new Vector2(0, _graphics.PreferredBackBufferHeight - 112);
            baseScale = (_graphics.PreferredBackBufferHeight * 0.125f) / _baseTexture.Height;
        }

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();

            bool isMouseClicked = currentMouseState.LeftButton == ButtonState.Pressed &&
                                  _previousMouseState.LeftButton == ButtonState.Released;

            if (_displayPlayer1 || _displayPlayer2)
            {
                // Allow clicking on the sides to return to the About scene
                if (isMouseClicked)
                {
                    _displayPlayer1 = false;
                    _displayPlayer2 = false;
                }
            }
            else
            {
                // Handle button clicks here
                if (isMouseClicked)
                {
                    if (_player1Hitbox.Contains(currentMouseState.Position))
                    {
                        _displayPlayer1 = true;
                    }
                    else if (_player2Hitbox.Contains(currentMouseState.Position))
                    {
                        _displayPlayer2 = true;
                    }
                    else if (_menuButtonHitbox.Contains(currentMouseState.Position))
                    {
                        _sceneManager.ChangeScene("MenuScene");
                    }
                }
            }
            _previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.Draw(_baseTexture, new Rectangle(0, _graphics.PreferredBackBufferHeight - (int)(_baseTexture.Height * baseScale), _graphics.PreferredBackBufferWidth, (int)(_baseTexture.Height * baseScale)), Color.White);
            spriteBatch.Draw(_logoTexture, _logoPosition, Color.White);
            spriteBatch.Draw(_birdTexture, _birdPosition, Color.White);

            if (_displayPlayer1)
            {
                // Center the picture
                Vector2 player1PicturePosition = new Vector2((_graphics.PreferredBackBufferWidth - _player1.Width) / 2, (_graphics.PreferredBackBufferHeight - _player1.Height) / 2 + 50);
                spriteBatch.Draw(_player1, player1PicturePosition, Color.White);
            }
            else if (_displayPlayer2)
            {
                // Center the picture
                Vector2 player2PicturePosition = new Vector2((_graphics.PreferredBackBufferWidth - _player2.Width) / 2, (_graphics.PreferredBackBufferHeight - _player2.Height) / 2 + 50);
                spriteBatch.Draw(_player2, player2PicturePosition, Color.White);
            }
            else
            {
                // Draw the About scene elements
                spriteBatch.Draw(_creatorsTexture, _creatorsPosition, Color.White);
                spriteBatch.Draw(_player1Texture, _player1Position, Color.White);
                spriteBatch.Draw(_player2Texture, _player2Position, Color.White);
                spriteBatch.Draw(_menuButtonTexture, _menuButtonPosition, Color.White);
            }

            spriteBatch.End();
        }
    }
}
