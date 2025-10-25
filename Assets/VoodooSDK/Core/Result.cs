namespace VoodooSDK.Core
{
    public class Result<T>
    {
        public T Payload;
        public bool Success;
        public string Message;
        
        public static Result<T> SuccessResult(T payload) => new()
        {
            Payload = payload,
            Success = true
        };
        public static Result<T> FailedResult(string message) => new() {Success = false, Message = message};
    }
}