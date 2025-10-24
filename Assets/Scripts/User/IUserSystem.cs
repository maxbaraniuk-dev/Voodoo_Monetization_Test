using Game.Level;
using Infrastructure;

namespace User
{
    public interface IUserSystem
    {
        UserData GetUserData();
        void SaveUserData();
        void OpenDifficultyLevel(DifficultyLevel difficultyLevel);
    }
}