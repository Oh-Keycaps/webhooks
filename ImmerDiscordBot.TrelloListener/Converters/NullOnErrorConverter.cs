using System;
using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.Converters
{
    public class NullOnErrorConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException($"Unnecessary because {nameof(CanWrite)} is false.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                return serializer.Deserialize(reader, objectType);
            }
            catch
            {
                return null;
            }
        }

        public override bool CanConvert(Type objectType) => true;
    }
}
