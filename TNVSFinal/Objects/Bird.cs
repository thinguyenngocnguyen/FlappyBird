using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TNVSFinal.Objects
{
    public class Bird
    {
        Texture2D _textureMidFlap;
        Texture2D _textureUpFlap;
        Texture2D _textureDownFlap;
        Texture2D _currentTexture;
        Vector2 _position;
        float _velocity;

        public Bird(Texture2D textureMidFlap, Texture2D textureUpFlap, Texture2D textureDownFlap)
        {
            _textureMidFlap = textureMidFlap;
            _textureUpFlap = textureUpFlap;
            _textureDownFlap = textureDownFlap;
            _currentTexture = textureMidFlap;
            _position = new Vector2(100, 200);
            _velocity = 0;
        }

        public void Update(GameTime gameTime, bool isFlapping)
        {
            _velocity += 0.5f;
            _position.Y += _velocity;

            if (isFlapping)
            {
                _velocity = -5f;
                _currentTexture = _textureUpFlap;
            }
            else
            {
                _currentTexture = _textureDownFlap;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_currentTexture, _position, Color.White);
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, _currentTexture.Width, _currentTexture.Height);
            }
        }

        public int TextureHeight
        {
            get { return _currentTexture.Height; }
        }

        public Vector2 Position
        {
            get { return _position; }
        }
    }
}