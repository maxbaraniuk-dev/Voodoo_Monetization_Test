using System.Collections.Generic;
using System.Linq;
using VoodooSDK.DTO.Offers;

namespace OffersSystem.Offers
{
    public class MultiplePersonalOffer : BaseOffer
    {
        public List<PurchaseItem> purchaseItems;
        public override void Show()
        {
            throw new System.NotImplementedException();
        }
        
        public static MultiplePersonalOffer FromDto(MultiplePersonalOfferData data)
        {
            return new MultiplePersonalOffer
            {
                segment = data.segment,
                trigger = data.trigger,
                purchaseItems = data.items.Select(PurchaseItem.FromDto).ToList()
            };
        }
    }
}