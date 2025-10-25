using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;
using User;
using VoodooSDK.Core;
using VoodooSDK.DTO;
using VoodooSDK.Game.Offers;
using VoodooSDK.Game.Purchasables;
using VoodooSDK.Store;

namespace VoodooSDK
{
    public static class MonetizationSDK
    {
        private const string LoginUrl = "https://th7bmd6lomsrpelxp3p7k4u34a0smwap.lambda-url.us-east-1.on.aws/";
        private const string UserStateUrl = "https://v2hah3mjyak4neneqhzyvotdpa0xpxnw.lambda-url.us-east-1.on.aws/";
        private const string AllOffersUrl = "https://gzrx4avbixaqryao7paoi7szt40dxddu.lambda-url.us-east-1.on.aws/";
        private const string PurchaseOfferUrl = "https://b2zumfx3g3orvwxy34r3crx3ye0xalnm.lambda-url.us-east-1.on.aws/";
        private const string AllStorePackagesUrl = "https://ornjmxpww64x6jkt27bf7jkcmm0fkgcd.lambda-url.us-east-1.on.aws/";
        private const string PurchasePackageUrl = "https://fwmo4m5zraasmingcwb4ktzkni0saetz.lambda-url.us-east-1.on.aws/";
        
        private static string _authToken;
        
        //Server API
        
        
        public static async UniTask<Result<Empty>> AuthenticateUser(string userId)
        {
            var authRequest = new AuthRequest
            {
                userId = userId
            };
            var response = await SendRequest<AuthResponse>(LoginUrl, RequestMethodType.Post, authRequest);
            if (!response.Success)
                return Result<Empty>.FailedResult(response.Message);

            if (string.IsNullOrEmpty(response.Payload.token))
                return Result<Empty>.FailedResult("Token is empty");
            
            _authToken = response.Payload.token;
            return Result<Empty>.SuccessResult(Empty.Default);
        }
        
        public static async UniTask<Result<UserData>> GetUserState()
        {
            var response = await SendRequest<UserData>(UserStateUrl);
            if (!response.Success)
                return Result<UserData>.FailedResult(response.Message);

            return response.Payload == null 
                       ? Result<UserData>.FailedResult("User data is empty") 
                       : Result<UserData>.SuccessResult(response.Payload);
        }
        
        public static async UniTask<Result<List<BaseOffer>>> GetAllOffers()
        {
            var response = await SendRequest<OffersResponse>(AllOffersUrl);
            if (!response.Success)
                return Result<List<BaseOffer>>.FailedResult(response.Message);

            if (response.Payload.offers == null)
                return Result<List<BaseOffer>>.FailedResult("Offers are empty");
                
            var res = response.Payload.offers.Select(container => container.offer).ToList();
            return Result<List<BaseOffer>>.SuccessResult(res);
        }
        
        public static async UniTask<Result<PurchaseItem>> PurchaseOfferItem(string purchaseItemId)
        {
            var requestData = new PurchaseOfferRequest
            {
                offerId = purchaseItemId
            };
            var response = await SendRequest<PurchaseItem>(PurchaseOfferUrl, RequestMethodType.Post, requestData);
            return !response.Success ? Result<PurchaseItem>.FailedResult(response.Message) : Result<PurchaseItem>.SuccessResult(response.Payload);
        }
        
        
        //Store API
        
        
        public static async UniTask<Result<StorePackage>> PurchasePackage(string packageId)
        {
            var requestData = new PurchasePackageRequest
            {
                packageId = packageId
            };
            var response = await SendRequest<StorePackage>(PurchasePackageUrl, RequestMethodType.Post, requestData);
            return !response.Success ? Result<StorePackage>.FailedResult(response.Message) : Result<StorePackage>.SuccessResult(response.Payload);
        }
        
        public static async UniTask<Result<List<StorePackage>>> GetStorePackages()
        {
            var response = await SendRequest<StorePackagesResponse>(AllStorePackagesUrl);
            if (!response.Success)
                return Result<List<StorePackage>>.FailedResult(response.Message);
            
            var packages = response.Payload.packages;
            
            return packages == null ? Result<List<StorePackage>>.FailedResult("Packages are empty") : Result<List<StorePackage>>.SuccessResult(packages);
        }
        
        private static async UniTask<Result<T>> SendRequest<T>(string url, RequestMethodType methodType = RequestMethodType.Get, object requestData = null)
        {
            var json = JsonConvert.SerializeObject(requestData);
            var dataToSend = new UTF8Encoding().GetBytes(json);
            
            var request = new UnityWebRequest(url, methodType.ToString());
            request.uploadHandler = new UploadHandlerRaw(dataToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {_authToken}");

            await request.SendWebRequest();
            
            switch (request.responseCode)
            {
                case 200:
                    var data = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                    return data == null 
                               ? Result<T>.FailedResult("Data model is invalid") 
                               : Result<T>.SuccessResult(data);
                case 401:
                    return Result<T>.FailedResult($"Errormessage: {request.error}, Error code: {request.responseCode}");
                default:
                    return Result<T>.FailedResult($"Errormessage: {request.error}, Error code: {request.responseCode}");
            }
        }
    }
}