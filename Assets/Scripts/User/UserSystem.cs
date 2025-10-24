using Events;
using Game.Level;
using Infrastructure;
using SaveLoad;
using UnityEngine;
using Zenject;

namespace User
{
    public class UserSystem : IUserSystem, ISystem
    {
        [Inject] ISaveSystem _saveSystem;
        private UserData _userData;
        public void Initialize()
        {
            _userData = _saveSystem.LoadUserData();
            EventsMap.Subscribe<DifficultyLevel>(GameEvents.OnOpenDifficultyLevel, OpenDifficultyLevel);
        }
   
        public void Dispose()
        {
            EventsMap.Unsubscribe<DifficultyLevel>(GameEvents.OnOpenDifficultyLevel, OpenDifficultyLevel);
        }

        public UserData GetUserData()
        {
            return _userData;
        }

        public void SaveUserData()
        {
            _saveSystem.SaveUserData(_userData);
        }

        public void OpenDifficultyLevel(DifficultyLevel difficultyLevel)
        {
            _userData.openedLevels.Find(state => state.difficultyLevel == difficultyLevel).isOpened = true;
            _saveSystem.SaveUserData(_userData);
        }
    }
}
