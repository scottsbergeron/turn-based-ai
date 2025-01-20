using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TurnBasedGame.Graphics;
using TurnBasedGame.Input;

namespace TurnBasedGame.Models
{
    public class Battle(ILevel level)
    {
        private readonly ILevel _level = level;
        private readonly InputHandler _inputHandler = new InputHandler();
        private HexRenderer _renderer = null!;
        
        private const float CAMERA_MOVE_SPEED = 500f;
        private const float CAMERA_PAN_SPEED = 1.0f;
        private const float ZOOM_SPEED = 0.1f;

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            _renderer = new HexRenderer(graphicsDevice);
            _level.Initialize();

            // Calculate map bounds based on hex grid size
            float mapWidth = _level.Map.Width * HexRenderer.HEX_SIZE * MathF.Sqrt(3);
            float mapHeight = _level.Map.Height * HexRenderer.HEX_SIZE * 1.5f;
            
            // Add some padding around the edges
            const float PADDING = 100f;
            Rectangle bounds = new Rectangle(
                (int)(-mapWidth/2 - PADDING),
                (int)(-mapHeight/2 - PADDING),
                (int)(mapWidth + PADDING * 2),
                (int)(mapHeight + PADDING * 2)
            );
            
            _renderer.Camera.SetBounds(bounds);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            _inputHandler.Update();

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

            // Update camera position
            _renderer.Camera.Update(deltaTime);
            
            _level.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _renderer.DrawHexMap(spriteBatch, _level.Map);
        }
    }
} 