using System.Collections.Generic;

namespace VoodooSDK.DTO.Purchasables
{
    public struct PurchaseItemData
    {
        public string id;
        public List<Reward> rewards;
        public string packageId;
        public bool isFree;
    }
}