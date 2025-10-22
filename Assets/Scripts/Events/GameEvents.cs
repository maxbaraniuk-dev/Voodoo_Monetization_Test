namespace Events
{
    public abstract class GameEvents
    {
        public static readonly string OnTimeUpdated = nameof(OnTimeUpdated);
        public static readonly string OnPlayerDistanceUpdated = nameof(OnPlayerDistanceUpdated);
        public static readonly string OnTargetReached = nameof(OnTargetReached);
    }
}