using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;
using User;
using VoodooSDK.Core;
using VoodooSDK.DTO;
using VoodooSDK.DTO.Auth;
using VoodooSDK.DTO.Offers;
using VoodooSDK.DTO.Purchasables;
using VoodooSDK.DTO.Store;

namespace VoodooSDK
{
    /// <summary>
    /// Provides high-level methods to interact with the backend monetization services.
    /// </summary>
    /// <remarks>
    /// This SDK wraps UnityWebRequest calls to the backend and returns strongly-typed results wrapped into
    /// <see cref="Result{T}"/>. Use <see cref="AuthenticateUser(string)"/> first to acquire an auth token
    /// for subsequent calls that require authorization.
    /// </remarks>
    public static class MonetizationServer
    {
        private const string LoginUrl = "https://th7bmd6lomsrpelxp3p7k4u34a0smwap.lambda-url.us-east-1.on.aws/";
        private const string UserStateUrl = "https://v2hah3mjyak4neneqhzyvotdpa0xpxnw.lambda-url.us-east-1.on.aws/";
        private const string AllOffersUrl = "https://gzrx4avbixaqryao7paoi7szt40dxddu.lambda-url.us-east-1.on.aws/";
        private const string PurchaseOfferUrl = "https://b2zumfx3g3orvwxy34r3crx3ye0xalnm.lambda-url.us-east-1.on.aws/";
        
        //Server API
        
        /// <summary>
        /// Authenticates the given user with the backend and stores the received auth token for subsequent requests.
        /// </summary>
        /// <param name="userId">Unique identifier of the player within your game.</param>
        /// <returns>
        /// <see cref="Result{T}"/> with <see cref="Empty"/> payload when authentication succeeds.
        /// Returns a failed result if the network call fails or the backend returns an empty token.
        /// </returns>
        /// <remarks>
        /// Must be called before any other API method that requires authorization headers.
        /// </remarks>
        public static async UniTask<Result<Empty>> AuthenticateUser(string userId)
        {
            var authRequest = new AuthRequest
            {
                userId = userId
            };
            var response = await NetworkProvider.SendRequest<AuthResponse>(LoginUrl, RequestMethodType.Post, authRequest);
            if (!response.Success)
                return Result<Empty>.FailedResult(response.Message);

            if (string.IsNullOrEmpty(response.Payload.token))
                return Result<Empty>.FailedResult("Token is empty");
            
            NetworkProvider.SetAuthToken(response.Payload.token);
            return Result<Empty>.SuccessResult(Empty.Default);
        }
        
        /// <summary>
        /// Retrieves the current user state from the backend.
        /// </summary>
        /// <returns>
        /// <see cref="Result{T}"/> with <see cref="UserData"/> on success; a failed result if the request fails
        /// or the backend returns an empty payload.
        /// </returns>
        /// <remarks>
        /// Requires a valid authentication token. Call <see cref="AuthenticateUser(string)"/> first.
        /// </remarks>
        public static async UniTask<Result<UserData>> GetUserState()
        {
            var response = await NetworkProvider.SendRequest<UserData>(UserStateUrl);
            if (!response.Success)
                return Result<UserData>.FailedResult(response.Message);

            return response.Payload == null 
                       ? Result<UserData>.FailedResult("User data is empty") 
                       : Result<UserData>.SuccessResult(response.Payload);
        }
        
        /// <summary>
        /// Fetches all available offers for the authenticated user.
        /// </summary>
        /// <returns>
        /// <see cref="Result{T}"/> with a list of <see cref="BaseOfferData"/> when successful; a failed result if the
        /// request fails or the backend returns an empty list.
        /// </returns>
        /// <remarks>
        /// Requires prior authentication via <see cref="AuthenticateUser(string)"/>.
        /// </remarks>
        public static async UniTask<Result<List<BaseOfferData>>> GetAllOffers()
        {
            var response = await NetworkProvider.SendRequest<OffersResponse>(AllOffersUrl);
            if (!response.Success)
                return Result<List<BaseOfferData>>.FailedResult(response.Message);

            if (response.Payload.offers == null)
                return Result<List<BaseOfferData>>.FailedResult("Offers are empty");
                
            var res = response.Payload.offers.Select(container => container.offer).ToList();
            return Result<List<BaseOfferData>>.SuccessResult(res);
        }
        
        /// <summary>
        /// Attempts to purchase a specific offer item by its identifier.
        /// </summary>
        /// <param name="purchaseItemId">The identifier of the offer item to purchase.</param>
        /// <returns>
        /// <see cref="Result{T}"/> with a <see cref="PurchaseItemData"/> payload on success; otherwise a failed result with an error message.
        /// </returns>
        /// <remarks>
        /// Requires prior authentication via <see cref="AuthenticateUser(string)"/>.
        /// </remarks>
        public static async UniTask<Result<PurchaseItemData>> PurchaseOfferItem(string purchaseItemId)
        {
            var requestData = new PurchaseItemRequest
            {
                offerId = purchaseItemId
            };
            var response = await NetworkProvider.SendRequest<PurchaseItemData>(PurchaseOfferUrl, RequestMethodType.Post, requestData);
            return !response.Success ? Result<PurchaseItemData>.FailedResult(response.Message) : Result<PurchaseItemData>.SuccessResult(response.Payload);
        }
    }
}