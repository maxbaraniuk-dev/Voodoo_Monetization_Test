using System;
using Cysharp.Threading.Tasks;
using Events;
using Game;
using Game.Level;
using Infrastructure;
using UI;
using VoodooSDK;
using VoodooSDK.DTO.Offers;
using VoodooSDK.DTO.Purchasables;
using Zenject;

namespace User
{
    public class UserSystem : IUserSystem, ISystem
    {
        [Inject] GameConfig _gameConfig;
        [Inject] IUISystem _uiSystem;
        private UserData _userData;
        public void Initialize()
        {
            EventsMap.Subscribe<DifficultyLevel>(GameEvents.TryUnlockDifficultyLevel, TryUnlockDifficultyLevel);
        }
   
        public void Dispose()
        {
            EventsMap.Unsubscribe<DifficultyLevel>(GameEvents.TryUnlockDifficultyLevel, TryUnlockDifficultyLevel);
        }

        public async UniTask<Result> LoadUserData()
        {
            var result = await MonetizationServer.GetUserState();
            if (!result.Success)
                return Result.FailedResult(result.Message);
            
            _userData = result.Payload;
            return Result.SuccessResult();
        }

        public UserData GetUserData()
        {
            return _userData;
        }

        public void AddReward(Reward reward)
        {
            switch (reward.type)
            {
                case RewardType.Coins:
                    _userData.coins += reward.value;
                    break;
                case RewardType.Stars:
                    _userData.stars += reward.value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void TryUnlockDifficultyLevel(DifficultyLevel difficultyLevel)
        {
            if (_userData.stars < _gameConfig.upLockLevelsStarsCost || _userData.coins < _gameConfig.upLockLevelsCoinsCost)
                EventsMap.Dispatch(GameEvents.OfferTrigger, OfferTrigger.OutOfResources);
            else
                UnlockLevel(difficultyLevel);
        }
        
        void UnlockLevel(DifficultyLevel difficultyLevel)
        {
            _userData.stars -= _gameConfig.upLockLevelsStarsCost;
            _userData.coins -= _gameConfig.upLockLevelsCoinsCost;
            
            _userData.openedLevels.Find(state => state.difficultyLevel == difficultyLevel).isOpened = true;
            _uiSystem.GetView<DifficultySelectDialog>().UpdateView(_userData.openedLevels);
            _uiSystem.GetView<LobbyView>().UpdateView(_userData);
        }
    }
}
