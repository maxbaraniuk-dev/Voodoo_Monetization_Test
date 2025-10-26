using System;
using VoodooSDK.DTO.Purchasables;

namespace VoodooSDK.DTO.Offers
{
    [Serializable]
    public class PersonalOfferData : BaseOfferData
    {
        public PurchaseItemData item;
    }
}