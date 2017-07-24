using System;
using Bridge.Contract.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bridge.Contract
{
    public class CombineConfig
    {
        public bool Enabled
        {
            get; set;
        }

        public bool NoReferenced
        {
            get; set;
        }
    }

    public class CombineConfigConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var config = value as CombineConfig;

            if (config == null)
            {
                return;
            }
            else if (!config.NoReferenced)
            {
                serializer.Serialize(writer, config.Enabled);
            }
            else
            {
                var s = JObject.FromObject(config);
                s.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Boolean)
            {
                var enabled = serializer.Deserialize<bool>(reader);

                return new CombineConfig() { Enabled = enabled };
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                var config = new CombineConfig();

                serializer.Populate(reader, config);
                existingValue = config;

                return existingValue;
            }
            else if (reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.None)
            {
                return existingValue;
            }

            throw new JsonReaderException(
               string.Format(
                   Messages.Exceptions.ERROR_CONFIG_DESERIALIZATION_NODE,
                   "CombineConfig. It should be either bool value an object { ... }.")
               );
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(CombineConfig).IsAssignableFrom(objectType);
        }
    }
}
