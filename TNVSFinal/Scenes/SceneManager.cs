using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace TNVSFinal.Scenes
{
    public class SceneManager
    {
        private Dictionary<string, IScene> _scenes;
        private IScene _currentScene;

        public SceneManager()
        {
            _scenes = new Dictionary<string, IScene>();
        }

        public void AddScene(string name, IScene scene)
        {
            _scenes[name] = scene;
        }

        public void ChangeScene(string name)
        {
            _currentScene = _scenes[name];
            _currentScene.Initialize();
            _currentScene.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            _currentScene?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScene?.Draw(spriteBatch);
        }
    }
}
