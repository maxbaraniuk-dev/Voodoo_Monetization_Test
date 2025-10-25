using System.Collections.Generic;
using VoodooSDK.Game.Purchasables;

namespace VoodooSDK.Game.Offers
{
    public class EndlessOffer : BaseOffer
    {
        public List<PurchaseItem> items;
        public int currentItemIndex;
    }
}