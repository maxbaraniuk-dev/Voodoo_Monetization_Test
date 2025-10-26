using Cysharp.Threading.Tasks;
using Infrastructure;

namespace OffersSystem
{
    public interface IOffersSystem
    {
        UniTask<Result> LoadOffers();
    }
}