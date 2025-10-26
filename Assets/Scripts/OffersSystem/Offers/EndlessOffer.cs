using VoodooSDK.DTO.Offers;

namespace OffersSystem.Offers
{
    public class EndlessOffer : BaseOffer
    {
        public override void Show()
        {
            throw new System.NotImplementedException();
        }
        
        public static EndlessOffer FromDto(EndlessOfferData data)
        {
            return new EndlessOffer
            {
                segment = data.segment,
                trigger = data.trigger
            };
        }
    }
}