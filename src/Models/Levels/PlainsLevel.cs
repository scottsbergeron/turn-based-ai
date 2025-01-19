using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedGame.Graphics;
using TurnBasedGame.Input;
using System;

namespace TurnBasedGame.Models.Levels
{
    public class PlainsLevel : ILevel
    {
        private HexRenderer _renderer;
        private InputHandler _inputHandler;
        public HexMap Map { get; private set; }
        public string Name => "Plains";

        private const float CAMERA_MOVE_SPEED = 500f;
        private const float CAMERA_PAN_SPEED = 1.0f;
        private const float ZOOM_SPEED = 0.1f;

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            _renderer = new HexRenderer(graphicsDevice);
            _inputHandler = new InputHandler();
            Map = new HexMap(5);
            
            // Configure the plains level terrain with some variety
            Random random = new Random();
            foreach (var tile in Map.Tiles.Values)
            {
                // Create a mostly plains map with some random features
                var roll = random.NextDouble();
                tile.Terrain = roll switch
                {
                    < 0.7 => TerrainType.Plains,
                    < 0.85 => TerrainType.Forest,
                    < 0.95 => TerrainType.Mountain,
                    _ => TerrainType.Water
                };
            }
        }

        public void Update(GameTime gameTime)
        {
            _inputHandler.Update();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Handle WASD camera movement
            if (_inputHandler.IsActionActive(GameAction.CameraMoveUp))
                _renderer.Camera.Move(new Vector2(0, -CAMERA_MOVE_SPEED * deltaTime));
            if (_inputHandler.IsActionActive(GameAction.CameraMoveDown))
                _renderer.Camera.Move(new Vector2(0, CAMERA_MOVE_SPEED * deltaTime));
            if (_inputHandler.IsActionActive(GameAction.CameraMoveLeft))
                _renderer.Camera.Move(new Vector2(-CAMERA_MOVE_SPEED * deltaTime, 0));
            if (_inputHandler.IsActionActive(GameAction.CameraMoveRight))
                _renderer.Camera.Move(new Vector2(CAMERA_MOVE_SPEED * deltaTime, 0));

            // Handle middle mouse camera panning
            if (_inputHandler.IsActionActive(GameAction.CameraPan))
            {
                Vector2 panDelta = _inputHandler.GetActionValue(GameAction.CameraPan);
                _renderer.Camera.Move(-panDelta * CAMERA_PAN_SPEED);
            }

            // Handle zoom
            if (_inputHandler.IsActionActive(GameAction.CameraZoomIn))
                _renderer.Camera.AdjustZoom(ZOOM_SPEED);
            if (_inputHandler.IsActionActive(GameAction.CameraZoomOut))
                _renderer.Camera.AdjustZoom(-ZOOM_SPEED);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _renderer.DrawHexMap(spriteBatch, Map);
        }
    }
}
