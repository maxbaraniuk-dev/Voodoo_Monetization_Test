using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure;
using Logs;
using VoodooSDK.DTO.Store;
using Zenject;

namespace Store
{
    public class StoreProvider : IStore, ISystem
    {
        [Inject] ILog _log;
        private List<StorePackage> _packages;
        public void Initialize()
        {
            _log.Debug(() => "StoreProvider initialized");
        }
        
        public void Dispose()
        {
        }

        public async UniTask<Result> Purchase(string productId)
        {
            var res = await VoodooSDK.Store.PurchasePackage(productId);
            return !res.Success ? Result.FailedResult(res.Message) : Result.SuccessResult();
        }

        public async UniTask<Result> LoadAllProducts()
        {
            var res = await VoodooSDK.Store.GetStorePackages();
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