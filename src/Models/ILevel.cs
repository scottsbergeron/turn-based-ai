using Microsoft.Xna.Framework;

namespace TurnBasedGame.Models
{
    public interface ILevel
    {
        HexMap Map { get; }
        string Name { get; }
        void Initialize(Team playerTeam, Team enemyTeam);
        void Update(GameTime gameTime);
    }
}
