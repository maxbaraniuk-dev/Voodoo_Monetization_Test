namespace Infrastructure
{
    public interface IPayLoadedState<T> : IState
    {
        void SetPayload(T payload);
    }
}