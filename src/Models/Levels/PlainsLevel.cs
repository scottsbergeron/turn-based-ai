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

        public void Initialize()
        {
            // Configure the plains level terrain with some variety
            Random random = new Random();
            foreach (var tile in Map.Tiles.Values)
            {
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
