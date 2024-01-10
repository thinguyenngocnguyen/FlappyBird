using TNVSFinal.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using Microsoft.Xna.Framework.Media;


namespace TNVSFinal.Scenes
{
    public class GameScene : IScene
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Bird _bird;
        private List<Pipe> _pipes;
        private const float PipeSpawnTime = 1.5f;
        private const float PipeGapHeight = 100f;
        private float _pipeSpawnTimer = PipeSpawnTime;
        private Texture2D _gameOverTexture;
        private Texture2D _pipeTexture;
        private Texture2D _backgroundTexture;
        private Texture2D _baseTexture;
        private Texture2D _startScreenTexture;
        private Texture2D[] _numberTextures;
        private Texture2D _bgEndTexture;
        private Texture2D _medalBronzeTexture;
        private Texture2D _medalSilverTexture;
        private Texture2D _medalGoldTexture;
        private Texture2D _medalPlatinumTexture;
        private Texture2D _btnPlayAgainTexture;
        private Texture2D _btnRankingTexture;
        private Texture2D _btnExitTexture;
        private SoundEffect _fallSound;
        private SoundEffect _scoreSound;
        private SoundEffect _jumpSound;
        private KeyboardState oldState;
        private MouseState oldMouseState;
        private float baseScale;
        private int _score;
        private Random _random = new Random();
        private GameState _gameState;
        private int _bestScore = 0;
        private Vector2 playAgainButtonPosition;
        private Vector2 rankingButtonPosition;
        private Vector2 exitButtonPosition;
        public bool IsGameOver { get; private set; }
        private ContentManager _content;
        private SpriteFont _gameOverFont;
        private Song _gameSong;

        private SceneManager _sceneManager;
        public GameScene(GraphicsDeviceManager graphics, ContentManager content, SceneManager sceneManager)
        {
            _graphics = graphics;
            _content = content;
            _pipes = new List<Pipe>();
            _numberTextures = new Texture2D[10];
            _gameState = GameState.StartScreen;
            _sceneManager = sceneManager;
        }

        public void Initialize()
        {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            oldState = Keyboard.GetState();
        }

        public void LoadContent()
        {
            _backgroundTexture = _content.Load<Texture2D>("background-day");
            _baseTexture = _content.Load<Texture2D>("base");
            _pipeTexture = _content.Load<Texture2D>("pipe-green");
            _gameOverTexture = _content.Load<Texture2D>("gameover");
            _startScreenTexture = _content.Load<Texture2D>("message");
            _fallSound = _content.Load<SoundEffect>("fallSound");
            _scoreSound = _content.Load<SoundEffect>("scoreSound");
            _jumpSound = _content.Load<SoundEffect>("jumpSound");
            _bgEndTexture = _content.Load<Texture2D>("bg-end");
            _medalBronzeTexture = _content.Load<Texture2D>("medal-bronze");
            _medalSilverTexture = _content.Load<Texture2D>("medal-silver");
            _medalGoldTexture = _content.Load<Texture2D>("medal-gold");
            _medalPlatinumTexture = _content.Load<Texture2D>("medal-platinum");
            _btnPlayAgainTexture = _content.Load<Texture2D>("btnPlayAgain");
            _btnRankingTexture = _content.Load<Texture2D>("btnRankingLogo");
            _btnExitTexture = _content.Load<Texture2D>("btnExit");
            var birdTextureMidFlap = _content.Load<Texture2D>("yellowbird-midflap");
            var birdTextureUpFlap = _content.Load<Texture2D>("yellowbird-upflap");
            var birdTextureDownFlap = _content.Load<Texture2D>("yellowbird-downflap");
            _gameOverFont = _content.Load<SpriteFont>("GameOverFont");
            _bird = new Bird(birdTextureMidFlap, birdTextureUpFlap, birdTextureDownFlap);
            _gameSong = _content.Load<Song>("gameSong");

            Vector2 center = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            Vector2 bgEndPosition = center - new Vector2(_bgEndTexture.Width / 2, _bgEndTexture.Height / 2);

            float buttonsCenterX = (_btnPlayAgainTexture.Width + _btnRankingTexture.Width + _btnExitTexture.Width + 20) / 2;
            exitButtonPosition = new Vector2(center.X - buttonsCenterX + 110, rankingButtonPosition.Y + _btnRankingTexture.Height + 360);



            playAgainButtonPosition = bgEndPosition + new Vector2(50, _bgEndTexture.Height - _btnPlayAgainTexture.Height + 80);
            rankingButtonPosition = bgEndPosition + new Vector2(_bgEndTexture.Width - _btnRankingTexture.Width - 50, _bgEndTexture.Height - _btnRankingTexture.Height + 75);


            for (int i = 0; i < _numberTextures.Length; i++)
            {
                _numberTextures[i] = _content.Load<Texture2D>(i.ToString());
            }

            baseScale = (_graphics.PreferredBackBufferHeight * 0.125f) / _baseTexture.Height;
        }


        public void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();

            if (_gameState == GameState.Playing && MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(_gameSong);
            }

            if (_gameState == GameState.StartScreen)
            {
                if (newState.IsKeyDown(Keys.Enter) || newState.GetPressedKeys().Length > 0 || newMouseState.LeftButton == ButtonState.Pressed)
                {
                    _gameState = GameState.Playing;
                }
            }

            if (_gameState == GameState.Playing)
            {
                bool isKeyPressed = newState.IsKeyDown(Keys.Space) && !oldState.IsKeyDown(Keys.Space);
                bool isMouseClicked = newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;

                bool isFlapping = isKeyPressed || isMouseClicked;

                _bird.Update(gameTime, isFlapping);

                if (isFlapping)
                {
                    _jumpSound.Play();
                }

                _pipeSpawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_pipeSpawnTimer <= 0)
                {
                    SpawnPipe();
                    _pipeSpawnTimer = PipeSpawnTime;
                }

                for (int i = _pipes.Count - 1; i >= 0; i--)
                {
                    Pipe pipe = _pipes[i];
                    pipe.Update(gameTime);

                    if (_bird.BoundingBox.Intersects(pipe.BoundingBox))
                    {
                        _fallSound.Play();
                        _gameState = GameState.GameOver;
                        IsGameOver = true;
                    }

                    if (!pipe.IsTopPipe && !pipe.HasScored && pipe.Position.X + _pipeTexture.Width < _bird.Position.X)
                    {
                        _score++;
                        _scoreSound.Play();
                        pipe.HasScored = true;
                    }

                    if (pipe.Position.X < -_pipeTexture.Width)
                    {
                        _pipes.RemoveAt(i);
                    }
                }

                float baseStartY = _graphics.PreferredBackBufferHeight - (_baseTexture.Height * baseScale);
                if (_bird.Position.Y + _bird.TextureHeight > baseStartY)
                {
                    _fallSound.Play();
                    _gameState = GameState.GameOver;
                    IsGameOver = true;
                }
            }

            if (_gameState == GameState.GameOver)
            {
                Rectangle playAgainButtonRect = new Rectangle(
                    (int)playAgainButtonPosition.X,
                    (int)playAgainButtonPosition.Y,
                    _btnPlayAgainTexture.Width,
                    _btnPlayAgainTexture.Height
                );

                Rectangle rankingButtonRect = new Rectangle(
                    (int)rankingButtonPosition.X,
                    (int)rankingButtonPosition.Y,
                    _btnRankingTexture.Width,
                    _btnRankingTexture.Height
                );

                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, _btnExitTexture.Width, _btnExitTexture.Height);

                if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (playAgainButtonRect.Contains(newMouseState.Position))
                    {
                        MediaPlayer.Stop();
                        RestartGame();
                        _sceneManager.ChangeScene("MenuScene");
                    }
                    else if (rankingButtonRect.Contains(newMouseState.Position))
                    {
                        MediaPlayer.Stop();
                        RestartGame();
                        _sceneManager.ChangeScene("RankingScene");
                    }
                    else if (exitButtonRect.Contains(newMouseState.Position))
                    {
                        Environment.Exit(0);
                    }
                }
            }

            oldState = newState;
            oldMouseState = newMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);

            if (_gameState == GameState.StartScreen)
            {
                Vector2 startScreenPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - _startScreenTexture.Width / 2,
                                                          _graphics.PreferredBackBufferHeight / 2 - _startScreenTexture.Height / 2);
                spriteBatch.Draw(_startScreenTexture, startScreenPosition, Color.White);
            }

            foreach (Pipe pipe in _pipes)
            {
                pipe.Draw(spriteBatch);
            }

            spriteBatch.Draw(_baseTexture, new Rectangle(0, _graphics.PreferredBackBufferHeight - (int)(_baseTexture.Height * baseScale), _graphics.PreferredBackBufferWidth, (int)(_baseTexture.Height * baseScale)), Color.White);

            _bird.Draw(spriteBatch);

            if (_gameState == GameState.Playing)
            {
                DrawScore(spriteBatch);
            }
            else if (IsGameOver)
            {
                DrawGameOverElements(spriteBatch);

                Vector2 gameOverPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - _gameOverTexture.Width / 2, _graphics.PreferredBackBufferHeight / 2 - _gameOverTexture.Height / 2 - 130);
                spriteBatch.Draw(_gameOverTexture, gameOverPosition, Color.White);

                spriteBatch.Draw(_btnExitTexture, exitButtonPosition, Color.White);
            }
            spriteBatch.End();
        }

        private void DrawScore(SpriteBatch spriteBatch)
        {
            string scoreStr = _score.ToString();
            Vector2 position = new Vector2(_graphics.PreferredBackBufferWidth / 2, 30);
            int totalWidth = 0;

            foreach (char c in scoreStr)
            {
                totalWidth += _numberTextures[c - '0'].Width;
            }

            position.X -= totalWidth / 2;

            foreach (char c in scoreStr)
            {
                Texture2D digitTexture = _numberTextures[c - '0'];
                spriteBatch.Draw(digitTexture, position, Color.White);
                position.X += digitTexture.Width;
            }
        }

        private void DrawGameOverElements(SpriteBatch spriteBatch)
        {
            Vector2 center = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            Vector2 bgEndPosition = center - new Vector2(_bgEndTexture.Width / 2, _bgEndTexture.Height / 2);

            spriteBatch.Draw(_bgEndTexture, bgEndPosition, Color.White);

            Texture2D medalTexture = GetMedalTexture(_score);
            Vector2 medalPosition = new Vector2(bgEndPosition.X + _bgEndTexture.Width / 4 - medalTexture.Width / 2 - 30, bgEndPosition.Y + 50);
            spriteBatch.Draw(medalTexture, medalPosition, Color.White);

            Vector2 medalLabelPosition = (medalPosition) + new Vector2(-_gameOverFont.MeasureString("Medal").X + 170 / 2, -56);
            spriteBatch.DrawString(_gameOverFont, "Medal", medalLabelPosition, Color.Orange);


            Vector2 scoreTextPosition = bgEndPosition + new Vector2(310, 40);
            Vector2 scoreLabelPosition = scoreTextPosition + new Vector2(-_gameOverFont.MeasureString("Score").X / 2, -46);
            spriteBatch.DrawString(_gameOverFont, "Score", scoreLabelPosition, Color.Orange);

            Vector2 bestTextPosition = bgEndPosition + new Vector2(310, 120);
            Vector2 bestLabelPosition = bestTextPosition + new Vector2(-_gameOverFont.MeasureString("Best").X / 2, -46);
            spriteBatch.DrawString(_gameOverFont, "Best", bestLabelPosition, Color.Orange);

            DrawGameOverScore(spriteBatch, _score, scoreTextPosition);
            _bestScore = Math.Max(_score, _bestScore);
            DrawGameOverScore(spriteBatch, _bestScore, bestTextPosition);

            spriteBatch.Draw(_btnPlayAgainTexture, playAgainButtonPosition, Color.White);
            spriteBatch.Draw(_btnRankingTexture, rankingButtonPosition, Color.White);
        }

        private Texture2D GetMedalTexture(int score)
        {
            if (score >= 31) return _medalPlatinumTexture;
            if (score >= 21) return _medalGoldTexture;
            if (score >= 11) return _medalSilverTexture;
            return _medalBronzeTexture;
        }

        private void DrawGameOverScore(SpriteBatch spriteBatch, int score, Vector2 position)
        {
            string scoreStr = score.ToString();
            int totalWidth = scoreStr.Sum(c => _numberTextures[c - '0'].Width);
            Vector2 scorePosition = position - new Vector2(totalWidth / 2, 0);

            foreach (char c in scoreStr)
            {
                Texture2D digitTexture = _numberTextures[c - '0'];
                spriteBatch.Draw(digitTexture, scorePosition, Color.White);
                scorePosition.X += digitTexture.Width;
            }
        }

        private void SpawnPipe()
        {
            int playableArea = _graphics.PreferredBackBufferHeight - (int)(_baseTexture.Height * baseScale);
            float gapY = _random.Next(0, playableArea - (int)PipeGapHeight);

            Vector2 topPipePosition = new Vector2(_graphics.PreferredBackBufferWidth, gapY - _pipeTexture.Height);
            _pipes.Add(new Pipe(_pipeTexture, topPipePosition, true));

            Vector2 bottomPipePosition = new Vector2(_graphics.PreferredBackBufferWidth, gapY + PipeGapHeight);
            _pipes.Add(new Pipe(_pipeTexture, bottomPipePosition, false));
        }

        private void RestartGame()
        {
            _pipes.Clear();

            var birdTextureMidFlap = _content.Load<Texture2D>("yellowbird-midflap");
            var birdTextureUpFlap = _content.Load<Texture2D>("yellowbird-upflap");
            var birdTextureDownFlap = _content.Load<Texture2D>("yellowbird-downflap");

            _bird = new Bird(birdTextureMidFlap, birdTextureUpFlap, birdTextureDownFlap);

            _score = 0;
            IsGameOver = false;
            _gameState = GameState.StartScreen;
            _pipeSpawnTimer = PipeSpawnTime;
        }

        private enum GameState
        {
            StartScreen,
            Playing,
            GameOver
        }
    }
}
