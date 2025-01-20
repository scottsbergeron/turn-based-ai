using System.Collections.Generic;
using TurnBasedGame.Models.Units;

namespace TurnBasedGame.Models
{
    public class Team(string name, bool isPlayerControlled)
    {
        public string Name { get; } = name;
        public bool IsPlayerControlled { get; } = isPlayerControlled;
        public List<Unit> Units { get; } = [];
    }
}
