using System;
using System.Collections.Generic;

namespace VoodooSDK.DTO
{
    [Serializable]
    public struct OffersResponse
    {
        public List<OfferContainer> offers;
    }
}