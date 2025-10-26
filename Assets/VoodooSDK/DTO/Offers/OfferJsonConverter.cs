using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VoodooSDK.DTO.Offers
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
            
            BaseOfferData source = type switch
            {
                OfferType.Personal => jsonObject.ToObject<PersonalOfferData>(),
                OfferType.MultiplePersonal => jsonObject.ToObject<MultiplePersonalOfferData>(),
                OfferType.Chained => jsonObject.ToObject<ChainedOfferData>(),
                OfferType.Endless => jsonObject.ToObject<EndlessOfferData>(),
                _ => jsonObject.ToObject<PersonalOfferData>()
            };
            return source;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}