using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TNVSFinal.Objects
{
    public class Pipe
    {
        private Texture2D _texture;
        public Vector2 Position { get; private set; }
        public bool HasScored { get; set; }
        public bool IsTopPipe { get; private set; }


        public Pipe(Texture2D texture, Vector2 startPosition, bool isTopPipe)
        {
            _texture = texture;
            Position = startPosition;
            IsTopPipe = isTopPipe;
            HasScored = false;
        }

        public void Update(GameTime gameTime)
        {
            Position = new Vector2(Position.X - 2, Position.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsTopPipe)
            {
                spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.FlipVertically, 0f);
            }
            else
            {
                spriteBatch.Draw(_texture, Position, Color.White);
            }
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }
    }
}
