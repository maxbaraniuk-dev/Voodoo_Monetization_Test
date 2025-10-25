using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
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
        private static string _authToken;
        
        //Server API
        
        public static async UniTask<Result<Empty>> AuthenticateUser(string userId)
        {
            var authRequest = new AuthRequest
            {
                userId = userId
            };
            var response = await SendRequest<AuthResponse>(ApiUrls.Login, RequestMethodType.Post, authRequest);
            if (!response.Success)
                return Result<Empty>.FailedResult(response.Message);

            if (string.IsNullOrEmpty(response.Payload.token))
                return Result<Empty>.FailedResult("Token is empty");
            
            _authToken = response.Payload.token;
            return Result<Empty>.SuccessResult(Empty.Default);
        }
        
        public static async UniTask<Result<UserData>> GetUserState()
        {
            var response = await SendRequest<UserData>(ApiUrls.UserState);
            if (!response.Success)
                return Result<UserData>.FailedResult(response.Message);

            return response.Payload == null 
                       ? Result<UserData>.FailedResult("User data is empty") 
                       : Result<UserData>.SuccessResult(response.Payload);
        }
        
        public static async UniTask<Result<List<BaseOffer>>> GetAllOffers()
        {
            var response = await SendRequest<OffersResponse>(ApiUrls.AllOffers);
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
            var response = await SendRequest<PurchaseItem>(ApiUrls.PurchaseOffer, RequestMethodType.Post, requestData);
            if (!response.Success)
                return Result<PurchaseItem>.FailedResult(response.Message);
            
            return Result<PurchaseItem>.SuccessResult(response.Payload);
        }
        
        //Store API
        
        public static async UniTask<Result<StorePackage>> PurchasePackage(string packageId)
        {
            var requestData = new PurchasePackageRequest
            {
                packageId = packageId
            };
            var response = await SendRequest<StorePackage>(ApiUrls.PurchasePackage, RequestMethodType.Post, requestData);
            if (!response.Success)
                return Result<StorePackage>.FailedResult(response.Message);
            
            return Result<StorePackage>.SuccessResult(response.Payload);
        }
        
        public static async UniTask<Result<List<StorePackage>>> GetStorePackages()
        {
            var response = await SendRequest<StorePackagesResponse>(ApiUrls.AllStorePackages);
            if (!response.Success)
                return Result<List<StorePackage>>.FailedResult(response.Message);
            
            var packages = response.Payload.packages;
            
            if (packages == null)
                return Result<List<StorePackage>>.FailedResult("Packages are empty");
            
            return Result<List<StorePackage>>.SuccessResult(packages);
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
        
        public static void GenerateStorePackagesResponse()
        {
            var data = new StorePackagesResponse
            {
                packages = new List<StorePackage>
                {
                    new StorePackage()
                    {
                        id = "com.purchasable.1",
                        price = 1,
                        currency = "USD",
                        receipt = "123456789"
                    },
                    new StorePackage()
                    {
                        id = "com.purchasable.10",
                        price = 10,
                        currency = "USD",
                        receipt = "123456789"
                    }
                }
            };
            
            var json = JsonConvert.SerializeObject(data);
            Debug.Log(json);
        }
        
        public static void GeneratePurchaseItem()
        {
            var data = new PurchaseItem
            {
                packageId = "com.purchasable.1",
                isFree = false,
                purchased = true,
                rewards = new List<Reward>()
                {
                    new()
                    {
                        type = RewardType.Coins,
                        value = 1000
                    },
                    new()
                    {
                        type = RewardType.Stars,
                        value = 5
                    }
                }
            };
            
            var json = JsonConvert.SerializeObject(data);
            Debug.Log(json);
        }
    }
}