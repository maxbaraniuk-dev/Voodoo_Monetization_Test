using Cysharp.Threading.Tasks;
using Infrastructure;
using VoodooSDK.Game.Offers;

namespace Offers
{
    public interface IOffersSystem
    {
        UniTask<Result> LoadOffers();
    }
}