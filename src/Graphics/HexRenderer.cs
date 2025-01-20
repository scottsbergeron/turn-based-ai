using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TurnBasedGame.Models;

namespace TurnBasedGame.Graphics
{
    public class HexRenderer
    {
        public const float HEX_SIZE = 40f;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly BasicEffect _basicEffect;
        private Camera2D _camera;
        
        // Colors for different terrain types
        private static readonly Color PlainsColor = new(152, 251, 152); // Light green
        private static readonly Color ForestColor = new(34, 139, 34);   // Dark green
        private static readonly Color MountainColor = new(139, 137, 137); // Gray
        private static readonly Color WaterColor = new(65, 105, 225);   // Blue

        public Camera2D Camera => _camera;

        public HexRenderer(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _basicEffect = new BasicEffect(graphicsDevice)
            {
                VertexColorEnabled = true,
                World = Matrix.Identity,
                View = Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up)
            };
            _camera = new Camera2D(graphicsDevice);
        }

        public void DrawHexMap(SpriteBatch spriteBatch, HexMap map)
        {
            spriteBatch.End();

            _basicEffect.Projection = Matrix.CreateOrthographicOffCenter(
                0, _graphicsDevice.Viewport.Width,
                _graphicsDevice.Viewport.Height, 0,
                0, 1);
            _basicEffect.World = _camera.TransformMatrix;

            foreach (var tile in map.Tiles.Values)
            {
                var position = GetHexPosition(tile);
                var color = GetTerrainColor(tile.Terrain);
                DrawHex(position, color);
            }

            spriteBatch.Begin();
        }

        private void DrawHex(Vector2 center, Color color)
        {
            var vertices = new VertexPositionColor[18]; // 6 triangles * 3 vertices each
            
            // Create six triangles, all sharing the center point
            for (int i = 0; i < 6; i++)
            {
                // Rotate by 30 degrees (π/6) to make hexes pointy-topped
                float angle1 = (i * MathF.PI / 3f) + MathF.PI / 6f;
                float angle2 = ((i + 1) % 6 * MathF.PI / 3f) + MathF.PI / 6f;

                // Center point
                vertices[i * 3] = new VertexPositionColor(
                    new Vector3(center, 0),
                    color
                );

                // First outer point
                vertices[i * 3 + 1] = new VertexPositionColor(
                    new Vector3(
                        center.X + HEX_SIZE * MathF.Cos(angle1),
                        center.Y + HEX_SIZE * MathF.Sin(angle1),
                        0
                    ),
                    color
                );

                // Second outer point
                vertices[i * 3 + 2] = new VertexPositionColor(
                    new Vector3(
                        center.X + HEX_SIZE * MathF.Cos(angle2),
                        center.Y + HEX_SIZE * MathF.Sin(angle2),
                        0
                    ),
                    color
                );
            }

            // Draw the filled hexagon
            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawUserPrimitives(
                    PrimitiveType.TriangleList,
                    vertices,
                    0,
                    6  // Number of triangles
                );
            }
        }

        private Vector2 GetHexPosition(HexTile tile)
        {
            // Convert cube coordinates to world position
            float x = HEX_SIZE * (MathF.Sqrt(3) * tile.Q + MathF.Sqrt(3) / 2 * tile.R);
            float y = HEX_SIZE * (3f / 2 * tile.R);

            return new Vector2(x, y);
        }

        private Color GetTerrainColor(TerrainType terrain)
        {
            return terrain switch
            {
                TerrainType.Plains => PlainsColor,
                TerrainType.Forest => ForestColor,
                TerrainType.Mountain => MountainColor,
                TerrainType.Water => WaterColor,
                _ => Color.White
            };
        }
    }
}
