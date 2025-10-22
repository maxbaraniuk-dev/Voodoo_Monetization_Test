using System.Collections.Generic;
using Game.Level;
using Infrastructure;
using Logs;
using UnityEngine;
using User;

namespace SaveLoad
{
    public class SaveSystem : MonoBehaviour, ISaveSystem
    {
        private const string LevelsDataKey = "levels";
        private const string UserDataKey = "userData";

        public void Initialize()
        {
            Context.GetSystem<ILog>().Debug(() => "SaveSystem initialized");
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

        public void SaveUserData(UserData userData)
        {
            LocalStorage.Save(UserDataKey, userData);
        }

        public UserData LoadUserData()
        {
            var data = LocalStorage.Load<UserData>(UserDataKey);
            if (data != null) 
                return data;
            
            data = UserData.New();
            LocalStorage.Save(UserDataKey, data);
            return data;
        }

    }
}