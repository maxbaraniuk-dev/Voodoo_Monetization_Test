using Cysharp.Threading.Tasks;
using Infrastructure;
using VoodooSDK.Game.Purchasables;

namespace User
{
    public interface IUserSystem
    {
        UniTask<Result> LoadUserData();
        UserData GetUserData();
        void AddReward(Reward reward);
    }
}