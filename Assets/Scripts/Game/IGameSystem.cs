using Game.Level;
using Infrastructure;

namespace Game
{
    public interface IGameSystem : ISystem
    {
        public void StartNewGame(DifficultyLevel difficultyLevel);
        public void ExitGame();
    }
}