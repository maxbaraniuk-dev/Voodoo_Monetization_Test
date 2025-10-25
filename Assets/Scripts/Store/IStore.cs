using Cysharp.Threading.Tasks;
using Infrastructure;
using VoodooSDK.Store;

namespace Store
{
    public interface IStore
    {
        UniTask<Result> Purchase(string productId);
        UniTask<Result> LoadAllProducts();
        StorePackage GetPackage(string packageId);
    }
}