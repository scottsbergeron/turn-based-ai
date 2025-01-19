using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedGame.Graphics
{
    public class Camera2D
    {
        private Vector2 _position;
        private float _zoom;
        private readonly GraphicsDevice _graphicsDevice;

        public Vector2 Position => _position;
        public float Zoom => _zoom;
        public Matrix TransformMatrix { get; private set; }

        public Camera2D(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _position = Vector2.Zero;
            _zoom = 1.0f;
            UpdateMatrix();
        }

        public void Move(Vector2 delta)
        {
            _position += delta / _zoom; // Adjust movement speed based on zoom level
            UpdateMatrix();
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
            UpdateMatrix();
        }

        public void AdjustZoom(float zoomDelta)
        {
            _zoom = MathHelper.Clamp(_zoom + zoomDelta, 0.1f, 3f);
            UpdateMatrix();
        }

        private void UpdateMatrix()
        {
            // Create the transformation matrix
            // This will transform world coordinates into screen coordinates
            Matrix translation = Matrix.CreateTranslation(new Vector3(-_position, 0f));
            Matrix scale = Matrix.CreateScale(_zoom);
            Matrix origin = Matrix.CreateTranslation(new Vector3(
                _graphicsDevice.Viewport.Width * 0.5f,
                _graphicsDevice.Viewport.Height * 0.5f,
                0f));

            TransformMatrix = translation * scale * origin;
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Matrix inverse = Matrix.Invert(TransformMatrix);
            return Vector2.Transform(screenPosition, inverse);
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TransformMatrix);
        }
    }
}
