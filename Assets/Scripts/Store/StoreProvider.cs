using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;
using VoodooSDK;
using VoodooSDK.Store;

namespace Store
{
    public class StoreProvider : IStore, ISystem
    {
        private List<StorePackage> _packages;
        public void Initialize()
        {
        }
        
        public void Dispose()
        {
        }

        public async UniTask<Result> Purchase(string productId)
        {
            var res = await MonetizationSDK.PurchasePackage(productId);
            return !res.Success ? Result.FailedResult(res.Message) : Result.SuccessResult();
        }

        public async UniTask<Result> LoadAllProducts()
        {
            var res = await MonetizationSDK.GetStorePackages();
            if (!res.Success)
                return Result.FailedResult(res.Message);
            
            _packages = res.Payload;
            return Result.SuccessResult();
        }

        public StorePackage GetPackage(string packageId)
        {
            return _packages.Find(package => package.id == packageId);
        }
    }
}