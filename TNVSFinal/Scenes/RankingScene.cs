using TNVSFinal.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using TNVSFinal.Scenes;

namespace TNVSFinal.Scenes
{
    public class RankingScene : IScene
    {
        private GraphicsDeviceManager _graphics;
        private ContentManager _content;
        private SpriteBatch _spriteBatch;

        private Texture2D _backgroundTexture;
        private Texture2D _baseTexture;
        private Texture2D _logoTexture;
        private Texture2D _birdTexture;
        private Texture2D _listRankingTexture;
        private Texture2D _btnMenuTexture;
        private Texture2D _btnRankingsTexture;
        private SpriteFont _font;
        private SceneManager _sceneManager;

        private Vector2 _logoPosition;
        private Vector2 _birdPosition;
        private Vector2 _basePosition;
        private Vector2 _btnRankingsPosition;
        private Vector2 _listRankingPosition;
        private Vector2 _menuButtonPosition;
        private float baseScale;
        private List<int> _topScores;

        public RankingScene(GraphicsDeviceManager graphics, ContentManager content, SceneManager sceneManager)
        {
            _graphics = graphics;
            _content = content;
            _sceneManager = sceneManager;
            _topScores = new List<int> { 15, 10, 8, 7, 6 };
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
            _btnRankingsTexture = _content.Load<Texture2D>("btnRankings");
            _listRankingTexture = _content.Load<Texture2D>("listRanking");
            _btnMenuTexture = _content.Load<Texture2D>("btnMenuSmaller");
            _font = _content.Load<SpriteFont>("RankingFont");

            float logoCenter = (_graphics.PreferredBackBufferWidth - _logoTexture.Width) / 2;
            _logoPosition = new Vector2(logoCenter, 20);
            _birdPosition = new Vector2(logoCenter + _logoTexture.Width + 10, _logoPosition.Y + _logoTexture.Height / 2 - _birdTexture.Height / 2);

            _btnRankingsPosition = new Vector2((_graphics.PreferredBackBufferWidth - _btnRankingsTexture.Width) / 2,
                                                _birdPosition.Y + _birdTexture.Height + 58);

            _listRankingPosition = new Vector2((_graphics.PreferredBackBufferWidth - _listRankingTexture.Width) / 2,
                                               _btnRankingsPosition.Y + _btnRankingsTexture.Height + 8);

            _menuButtonPosition = new Vector2((_graphics.PreferredBackBufferWidth - _btnMenuTexture.Width) / 2,
                                              _listRankingPosition.Y + _listRankingTexture.Height + 8);

            _basePosition = new Vector2(0, _graphics.PreferredBackBufferHeight - _content.Load<Texture2D>("base").Height);
            baseScale = (_graphics.PreferredBackBufferHeight * 0.125f) / _content.Load<Texture2D>("base").Height;
        }

        public void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
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

            spriteBatch.Draw(_btnRankingsTexture, _btnRankingsPosition, Color.White);

            spriteBatch.Draw(_listRankingTexture, _listRankingPosition, Color.White);

            Vector2 scorePosition = _listRankingPosition + new Vector2(40, 18);
            for (int i = 0; i < _topScores.Count; i++)
            {
                spriteBatch.DrawString(_font, $"{_topScores[i]} points", scorePosition, Color.Black);
                scorePosition.Y += 26;
            }

            spriteBatch.Draw(_btnMenuTexture, _menuButtonPosition, Color.White);

            spriteBatch.End();
        }
    }
}