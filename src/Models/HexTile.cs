using System.Collections.Generic;

namespace TurnBasedGame.Models
{
    public class HexTile
    {
        // Cube coordinates
        public int Q { get; } // Column
        public int R { get; } // Row
        public int S { get; } // Third axis (Q + R + S = 0)
        
        // Tile properties
        public int Elevation { get; set; }
        public TerrainType Terrain { get; set; }
        public bool IsPassable { get; set; }

        // Adjacent tiles (will be set by HexMap)
        public Dictionary<HexDirection, HexTile> Neighbors { get; }

        public HexTile(int q, int r)
        {
            Q = q;
            R = r;
            S = -q - r; // Cube coordinate constraint
            Neighbors = new Dictionary<HexDirection, HexTile>();
            IsPassable = true;
            Terrain = TerrainType.Plains;
        }
    }

    public enum TerrainType
    {
        Plains,
        Forest,
        Mountain,
        Water
    }

    public enum HexDirection
    {
        TopRight,
        Right,
        BottomRight,
        BottomLeft,
        Left,
        TopLeft
    }
}
