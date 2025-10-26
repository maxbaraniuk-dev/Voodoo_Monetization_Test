using System;
using System.Collections.Generic;

namespace VoodooSDK.DTO.Offers
{
    [Serializable]
    public struct OffersResponse
    {
        public List<OfferContainer> offers;
    }
}