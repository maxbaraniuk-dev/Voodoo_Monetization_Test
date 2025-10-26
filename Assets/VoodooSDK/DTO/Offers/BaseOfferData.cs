using System;

namespace VoodooSDK.DTO.Offers
{
    [Serializable]
    public abstract class BaseOfferData
    {
        internal OfferType type;
        public OfferTrigger trigger;
        public string segment;
    }
}