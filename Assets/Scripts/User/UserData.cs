using System;
using System.Collections.Generic;
using Game.Level;

namespace User
{
    [Serializable]
    public class UserData
    {
        public string id;
        public List<LevelState> openedLevels;
        
        public static UserData New()
        {
            return new UserData
            {
                id = Guid.NewGuid().ToString(),
                openedLevels = new List<LevelState>
                {
                    new(){difficultyLevel = DifficultyLevel.Easy, isOpened = true},
                    new(){difficultyLevel = DifficultyLevel.Medium, isOpened = false},
                    new(){difficultyLevel = DifficultyLevel.Hard, isOpened = false}
                }
            };
        }
    }
    
    [Serializable]
    public class LevelState
    {
        public DifficultyLevel difficultyLevel;
        public bool isOpened;
    }
}