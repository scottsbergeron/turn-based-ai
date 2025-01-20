using Microsoft.Xna.Framework;

namespace TurnBasedGame.Models.Units
{
    public abstract class Unit(string name, int maxHealth)
    {
        public string Name { get; } = name;
        public int Health { get; protected set; } = maxHealth;
        public int MaxHealth { get; } = maxHealth;
        public HexTile Position { get; set; } = null!;

        public abstract void Initialize(HexTile position);

        public abstract void Update(GameTime gameTime);
    }
}
