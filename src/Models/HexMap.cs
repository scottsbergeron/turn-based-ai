using System;
using System.Collections.Generic;

namespace TurnBasedGame.Models
{
    public class HexMap
    {
        private Dictionary<(int Q, int R), HexTile> _tiles;
        public IReadOnlyDictionary<(int Q, int R), HexTile> Tiles => _tiles;

        public HexMap(int radius)
        {
            _tiles = new Dictionary<(int Q, int R), HexTile>();
            GenerateMap(radius);
            ConnectNeighbors();
        }

        private void GenerateMap(int radius)
        {
            for (int q = -radius; q <= radius; q++)
            {
                int r1 = Math.Max(-radius, -q - radius);
                int r2 = Math.Min(radius, -q + radius);
                
                for (int r = r1; r <= r2; r++)
                {
                    var tile = new HexTile(q, r);
                    _tiles.Add((q, r), tile);
                }
            }
        }

        private void ConnectNeighbors()
        {
            var directions = new (int Q, int R)[]
            {
                (1, 0),   // TopRight
                (1, -1),  // Right
                (0, -1),  // BottomRight
                (-1, 0),  // BottomLeft
                (-1, 1),  // Left
                (0, 1)    // TopLeft
            };

            foreach (var tile in _tiles.Values)
            {
                for (int i = 0; i < directions.Length; i++)
                {
                    var dir = directions[i];
                    var neighborCoord = (tile.Q + dir.Q, tile.R + dir.R);
                    
                    if (_tiles.TryGetValue(neighborCoord, out var neighbor))
                    {
                        tile.Neighbors[(HexDirection)i] = neighbor;
                    }
                }
            }
        }

        public HexTile? GetTile(int q, int r)
        {
            return _tiles.TryGetValue((q, r), out var tile) ? tile : null;
        }
    }
}
