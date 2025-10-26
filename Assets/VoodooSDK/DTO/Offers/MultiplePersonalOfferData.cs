using System;
using System.Collections.Generic;
using VoodooSDK.DTO.Purchasables;

namespace VoodooSDK.DTO.Offers
{
    [Serializable]
    public class MultiplePersonalOfferData : BaseOfferData
    {
        public List<PurchaseItemData> items;
    }
}