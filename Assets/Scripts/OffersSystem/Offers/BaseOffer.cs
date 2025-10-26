using System.Collections.Generic;
using VoodooSDK.DTO.Offers;

namespace OffersSystem.Offers
{
    public abstract class BaseOffer
    {
        public OfferTrigger trigger;
        public string segment;
        
        public static BaseOffer CreateFromData(BaseOfferData data)
        {
            BaseOffer offer = data switch
            {
                PersonalOfferData offerData => PersonalOffer.FromDto(offerData),
                MultiplePersonalOfferData offerData => MultiplePersonalOffer.FromDto(offerData),
                ChainedOfferData offerData => ChainedOffer.FromDto(offerData),
                EndlessOfferData offerData => EndlessOffer.FromDto(offerData),
                _                          => throw new System.Exception("Unknown offer type")
            };
            return offer;
        }
        
        public abstract void Show();
    }
}