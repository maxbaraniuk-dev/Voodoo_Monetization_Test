using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VoodooSDK.Game.Offers
{
    public class OfferJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);
            t.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject;
            try
            {
                jsonObject = JObject.Load(reader);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            var typeString = (string) jsonObject.Property("type");
            if (string.IsNullOrEmpty(typeString))
                throw new JsonReaderException($"Can't parse CardContentData, unknown type: \"{typeString}\".");
            
            int.TryParse(typeString, out var typeInt);
            var type = (OfferType) typeInt;
            
            BaseOffer source = type switch
            {
                OfferType.Personal => jsonObject.ToObject<PersonalOffer>(),
                OfferType.MultiplePersonal => jsonObject.ToObject<MultiplePersonalOffer>(),
                OfferType.Chained => jsonObject.ToObject<ChainedOffer>(),
                OfferType.Endless => jsonObject.ToObject<EndlessOffer>(),
                _ => jsonObject.ToObject<PersonalOffer>()
            };
            return source;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}