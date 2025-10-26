using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using VoodooSDK.Core;
using VoodooSDK.DTO.Store;

namespace VoodooSDK
{
    public static class Store
    {
        private const string AllStorePackagesUrl = "https://ornjmxpww64x6jkt27bf7jkcmm0fkgcd.lambda-url.us-east-1.on.aws/";
        private const string PurchasePackageUrl = "https://fwmo4m5zraasmingcwb4ktzkni0saetz.lambda-url.us-east-1.on.aws/";
        
        /// <summary>
        /// Purchases a store package by its identifier.
        /// </summary>
        /// <param name="packageId">The unique identifier of the store package to purchase.</param>
        /// <returns>
        /// <see cref="Result{T}"/> with a <see cref="StorePackage"/> payload on success; otherwise a failed result with details.
        /// </returns>
        /// <remarks>
        /// Requires prior authentication via <see cref="AuthenticateUser(string)"/>.
        /// </remarks>
        public static async UniTask<Result<StorePackage>> PurchasePackage(string packageId)
        {
            var requestData = new PurchasePackageRequest
            {
                packageId = packageId
            };
            var response = await NetworkProvider.SendRequest<StorePackage>(PurchasePackageUrl, RequestMethodType.Post, requestData);
            return !response.Success ? Result<StorePackage>.FailedResult(response.Message) : Result<StorePackage>.SuccessResult(response.Payload);
        }
        
        /// <summary>
        /// Retrieves the list of available store packages for the authenticated user.
        /// </summary>
        /// <returns>
        /// <see cref="Result{T}"/> with a list of <see cref="StorePackage"/> on success; otherwise a failed result
        /// containing an error message.
        /// </returns>
        /// <remarks>
        /// Requires prior authentication via <see cref="AuthenticateUser(string)"/>.
        /// </remarks>
        public static async UniTask<Result<List<StorePackage>>> GetStorePackages()
        {
            var response = await NetworkProvider.SendRequest<StorePackagesResponse>(AllStorePackagesUrl);
            if (!response.Success)
                return Result<List<StorePackage>>.FailedResult(response.Message);
            
            var packages = response.Payload.packages;
            
            return packages == null ? Result<List<StorePackage>>.FailedResult("Packages are empty") : Result<List<StorePackage>>.SuccessResult(packages);
        }
    }
}