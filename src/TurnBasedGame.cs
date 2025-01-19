using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TurnBasedGame.Models;
using TurnBasedGame.Models.Levels;
using TurnBasedGame.Input;

namespace TurnBasedGame
{
    public class TurnBasedGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ILevel _currentLevel;
        private InputHandler _inputHandler;

        public TurnBasedGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _inputHandler = new InputHandler();
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentLevel = new PlainsLevel();
            _currentLevel.Initialize(GraphicsDevice);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            _inputHandler.Update();

            if (_inputHandler.IsActionActive(GameAction.CameraPan))
            {
                Vector2 panDelta = _inputHandler.GetActionValue(GameAction.CameraPan);
                // Handle camera pan
            }

            if (_inputHandler.IsActionActive(GameAction.Select))
            {
                Vector2 clickPos = _inputHandler.GetActionValue(GameAction.Select);
                // Handle selection
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _currentLevel.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _currentLevel.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
