using Game.Level;
using Infrastructure;

namespace Game
{
    public interface IGameSystem
    {
        public void StartNewGame(DifficultyLevel difficultyLevel);
        public void ExitGame();
    }
}