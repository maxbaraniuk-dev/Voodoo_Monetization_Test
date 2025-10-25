using System;
using Newtonsoft.Json;
using VoodooSDK.Game.Offers;

namespace VoodooSDK.DTO
{
    [Serializable]
    public struct OfferContainer
    {
        [JsonConverter(typeof(OfferJsonConverter))]
        public BaseOffer offer;
    }
}