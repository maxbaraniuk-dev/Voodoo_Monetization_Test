namespace Events
{
    public abstract class GameEvents
    {
        public static readonly string TimeUpdated = nameof(TimeUpdated);
        public static readonly string PlayerDistanceUpdated = nameof(PlayerDistanceUpdated);
        public static readonly string TargetReached = nameof(TargetReached);
        public static readonly string TryUnlockDifficultyLevel = nameof(TryUnlockDifficultyLevel);
        public static readonly string OfferTrigger = nameof(OfferTrigger);
        public static readonly string PurchaseOfferItem = nameof(PurchaseOfferItem);
    }
}