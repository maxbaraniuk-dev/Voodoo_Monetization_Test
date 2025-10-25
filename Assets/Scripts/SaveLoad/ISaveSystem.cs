using Game.Level;
using Infrastructure;
using User;

namespace SaveLoad
{
    public interface ISaveSystem
    {
        void SaveCompletedLevel(LevelResultData levelResult);
        public AchievementsData LoadLevelsStat();
        string LoadUserId();
    }
}