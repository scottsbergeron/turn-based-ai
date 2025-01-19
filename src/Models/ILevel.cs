using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedGame.Models
{
    public interface ILevel
    {
        HexMap Map { get; }
        string Name { get; }
        void Initialize(GraphicsDevice graphicsDevice);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
