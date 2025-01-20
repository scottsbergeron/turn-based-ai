using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedGame.Graphics
{
    public class Camera2D
    {
        private Vector2 _position;
        private Vector2 _targetPosition;
        private float _zoom;
        private readonly GraphicsDevice _graphicsDevice;
        private Rectangle _bounds;
        private const float CAMERA_LERP_SPEED = 10f;  // Adjust this to change smoothing speed

        // Add zoom constraints
        private const float MIN_ZOOM = 0.5f;
        private const float MAX_ZOOM = 1.2f;

        public Vector2 Position => _position;
        public float Zoom => _zoom;
        public Matrix TransformMatrix { get; private set; }

        public Camera2D(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _position = Vector2.Zero;
            _targetPosition = Vector2.Zero;
            _zoom = 1.0f;
            _bounds = new Rectangle(0, 0, 0, 0);
            UpdateMatrix();
        }

        public void SetBounds(Rectangle bounds)
        {
            _bounds = bounds;
            ClampPosition();
        }

        public void Update(float deltaTime)
        {
            _position = Vector2.Lerp(_position, _targetPosition, CAMERA_LERP_SPEED * deltaTime);
            UpdateMatrix();
        }

        public void Move(Vector2 delta)
        {
            _targetPosition += delta / _zoom;
            ClampPosition();
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
            UpdateMatrix();
        }

        public void AdjustZoom(float zoomDelta)
        {
            _zoom = MathHelper.Clamp(_zoom + zoomDelta, MIN_ZOOM, MAX_ZOOM);
            UpdateMatrix();
            ClampPosition();
        }

        private void ClampPosition()
        {
            if (_bounds.Width == 0 || _bounds.Height == 0) return;

            // Calculate visible area based on zoom
            float visibleWidth = _graphicsDevice.Viewport.Width / _zoom;
            float visibleHeight = _graphicsDevice.Viewport.Height / _zoom;

            // Adjust bounds based on zoom level
            float minX = _bounds.Left + visibleWidth / 2;
            float maxX = _bounds.Right - visibleWidth / 2;
            float minY = _bounds.Top + visibleHeight / 2;
            float maxY = _bounds.Bottom - visibleHeight / 2;

            // If zoomed out too far, center the camera
            if (maxX < minX) _targetPosition.X = _bounds.Center.X;
            else _targetPosition.X = MathHelper.Clamp(_targetPosition.X, minX, maxX);

            if (maxY < minY) _targetPosition.Y = _bounds.Center.Y;
            else _targetPosition.Y = MathHelper.Clamp(_targetPosition.Y, minY, maxY);
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
