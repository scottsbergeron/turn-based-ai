using Microsoft.Xna.Framework;
using System;

namespace TurnBasedGame.Models.Levels
{
    public class PlainsLevel : ILevel
    {
        public HexMap Map { get; private set; }
        public string Name => "Plains";

        public PlainsLevel()
        {
            Map = new HexMap(5);
        }

        public void Initialize(Team playerTeam, Team enemyTeam)
        {
            // Recreate map with extra rows for spawn areas
            int baseRadius = 5;
            int extraRows = Math.Max(playerTeam.Units.Count, enemyTeam.Units.Count);
            Map = new HexMap(baseRadius + extraRows);

            // Configure the plains level terrain with some variety
            Random random = new();
            foreach (var tile in Map.Tiles.Values)
            {
                // Player spawn area (bottom of map)
                if (tile.R >= Map.Height/2 - playerTeam.Units.Count)
                {
                    tile.Terrain = TerrainType.Plains;
                    continue;
                }
                
                // Enemy spawn area (top of map)
                if (tile.R <= -Map.Height/2 + enemyTeam.Units.Count)
                {
                    tile.Terrain = TerrainType.Plains;
                    continue;
                }

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

        }
    }
}
