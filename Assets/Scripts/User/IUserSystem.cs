using Game.Level;
using Infrastructure;

namespace User
{
    public interface IUserSystem : ISystem
    {
        UserData GetUserData();
        void SaveUserData();
        void OpenDifficultyLevel(DifficultyLevel difficultyLevel);
    }
}