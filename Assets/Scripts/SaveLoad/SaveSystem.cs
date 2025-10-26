using System;
using System.Collections.Generic;
using Game.Level;
using Infrastructure;
using Logs;
using Zenject;

namespace SaveLoad
{
    public class SaveSystem : ISaveSystem, ISystem
    {
        [Inject] ILog _log;
        private const string LevelsDataKey = "levels";
        private const string UserDataKey = "userData";

        public void Initialize()
        {
            _log.Debug(() => "SaveSystem initialized");
        }
        
        public void Dispose() { }

        public void SaveCompletedLevel(LevelResultData levelResult)
        {
            var data = LocalStorage.Load<AchievementsData>(LevelsDataKey) ?? new AchievementsData{levels = new List<LevelResultData>()};
            data.levels.Add(levelResult);
            LocalStorage.Save(LevelsDataKey, data);
        }

        public AchievementsData LoadLevelsStat()
        {
            var data = LocalStorage.Load<AchievementsData>(LevelsDataKey) ?? new AchievementsData{levels = new List<LevelResultData>()};
            return data;
        }

        public string LoadUserId()
        {
            var data = LocalStorage.Load<string>(UserDataKey);
            if (data != null) 
                return data;
            
            _log.Debug(() => "User id not found, generating new one");
            data = Guid.NewGuid().ToString();
            LocalStorage.Save(UserDataKey, data);
            return data;
        }

    }
}