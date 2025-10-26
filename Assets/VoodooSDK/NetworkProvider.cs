using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;
using VoodooSDK.Core;

namespace VoodooSDK
{
    internal static class NetworkProvider
    {
        private static string _authToken;
        public static void SetAuthToken(string authToken) => _authToken = authToken;
        public static async UniTask<Result<T>> SendRequest<T>(string url, RequestMethodType methodType = RequestMethodType.Get, object requestData = null)
        {
            var json = JsonConvert.SerializeObject(requestData);
            var dataToSend = new UTF8Encoding().GetBytes(json);

            using var request = new UnityWebRequest(url, methodType.ToString());
            request.uploadHandler = new UploadHandlerRaw(dataToSend);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Ensure handlers are disposed when the request is disposed
            request.disposeUploadHandlerOnDispose = true;
            request.disposeDownloadHandlerOnDispose = true;

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