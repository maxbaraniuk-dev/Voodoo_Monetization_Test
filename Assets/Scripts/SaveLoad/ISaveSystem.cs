using Game;
using Game.Level;
using Infrastructure;
using User;

namespace SaveLoad
{
    public interface ISaveSystem : ISystem
    {
        void SaveCompletedLevel(LevelResultData levelResult);
        public AchievementsData LoadLevelsStat();
        void SaveUserData(UserData userData);
        UserData LoadUserData();
    }
}