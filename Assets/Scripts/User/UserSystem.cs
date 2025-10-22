using Game.Level;
using Infrastructure;
using SaveLoad;
using UnityEngine;

namespace User
{
    public class UserSystem : MonoBehaviour, IUserSystem
    {
        private UserData _userData;
        public void Initialize()
        {
            _userData = Context.GetSystem<ISaveSystem>().LoadUserData();
        }
   
        public void Dispose()
        {
            Voodoo.Monetization.Dispose();
        }

        public UserData GetUserData()
        {
            return _userData;
        }

        public void SaveUserData()
        {
            Context.GetSystem<ISaveSystem>().SaveUserData(_userData);
        }

        public void OpenDifficultyLevel(DifficultyLevel difficultyLevel)
        {
            _userData.openedLevels.Find(state => state.difficultyLevel == difficultyLevel).isOpened = true;
            Context.GetSystem<ISaveSystem>().SaveUserData(_userData);
        }
    }
}
