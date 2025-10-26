using System;
using Newtonsoft.Json;

namespace VoodooSDK.DTO.Offers
{
    [Serializable]
    public struct OfferContainer
    {
        [JsonConverter(typeof(OfferJsonConverter))]
        public BaseOfferData offer;
    }
}