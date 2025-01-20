using Microsoft.Xna.Framework;

namespace TurnBasedGame.Models
{
    public interface ILevel
    {
        HexMap Map { get; }
        string Name { get; }
        void Initialize();
        void Update(GameTime gameTime);
    }
}
