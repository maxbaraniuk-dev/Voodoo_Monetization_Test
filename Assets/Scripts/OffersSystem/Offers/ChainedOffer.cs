using System.Collections.Generic;
using System.Linq;
using VoodooSDK.DTO.Offers;

namespace OffersSystem.Offers
{
    public class ChainedOffer : BaseOffer
    {
        public List<PurchaseItem> purchaseItems;
        public int currentItemIndex;

        public override void Show()
        {
            throw new System.NotImplementedException();
        }

        public static ChainedOffer FromDto(ChainedOfferData offerData)
        {
            return new ChainedOffer
            {
                trigger = offerData.trigger,
                segment = offerData.segment,
                currentItemIndex = offerData.currentItemIndex,
                purchaseItems = offerData.items.Select(PurchaseItem.FromDto).ToList()
            };
        }
    }
}