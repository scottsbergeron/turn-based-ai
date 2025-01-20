namespace TurnBasedGame.Models
{
    public class Turn(Team[] teams)
    {
        private readonly Team[] _teams = teams;
        private int _currentTeamIndex = 0;

        public Team ActiveTeam => _teams[_currentTeamIndex];
        public bool IsPlayerTurn => ActiveTeam.IsPlayerControlled;

        public void EndTurn()
        {
            _currentTeamIndex = (_currentTeamIndex + 1) % _teams.Length;
        }
    }
}
