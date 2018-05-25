using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ElClimaUDEO.Model
{

    public partial class WeatherModel
    {
        [JsonProperty("query")]
        public Query Query { get; set; }
    }

    public partial class Query
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("results")]
        public Results Results { get; set; }
    }

    public partial class Results
    {
        [JsonProperty("channel")]
        public Channel Channel { get; set; }
    }

    public partial class Channel
    {
        [JsonProperty("units")]
        public Units Units { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("lastBuildDate")]
        public string LastBuildDate { get; set; }

        [JsonProperty("ttl")]
        public string Ttl { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }

        [JsonProperty("atmosphere")]
        public Atmosphere Atmosphere { get; set; }

        [JsonProperty("astronomy")]
        public Astronomy Astronomy { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("item")]
        public Item Item { get; set; }
    }

    public partial class Astronomy
    {
        [JsonProperty("sunrise")]
        public string Sunrise { get; set; }

        [JsonProperty("sunset")]
        public string Sunset { get; set; }
    }

    public partial class Atmosphere
    {
        [JsonProperty("humidity")]
        public string Humidity { get; set; }

        [JsonProperty("pressure")]
        public string Pressure { get; set; }

        [JsonProperty("rising")]
        public string Rising { get; set; }

        [JsonProperty("visibility")]
        public string Visibility { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("width")]
        public string Width { get; set; }

        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("long")]
        public string Long { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("pubDate")]
        public string PubDate { get; set; }

        [JsonProperty("condition")]
        public Condition Condition { get; set; }

        [JsonProperty("forecast")]
        public Forecast[] Forecast { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("guid")]
        public Guid Guid { get; set; }
    }

    public partial class Condition
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("temp")]
        public string Temp { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class Forecast
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("day")]
        public string Day { get; set; }

        [JsonProperty("high")]
        public string High { get; set; }

        [JsonProperty("low")]
        public string Low { get; set; }

        [JsonProperty("text")]
        public Text Text { get; set; }
    }

    public partial class Guid
    {
        [JsonProperty("isPermaLink")]
        public string IsPermaLink { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }
    }

    public partial class Units
    {
        [JsonProperty("distance")]
        public string Distance { get; set; }

        [JsonProperty("pressure")]
        public string Pressure { get; set; }

        [JsonProperty("speed")]
        public string Speed { get; set; }

        [JsonProperty("temperature")]
        public string Temperature { get; set; }
    }

    public partial class Wind
    {
        [JsonProperty("chill")]
        public string Chill { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("speed")]
        public string Speed { get; set; }
    }

    public enum Text { PartlyCloudy, Rain, ScatteredThunderstorms };

    public partial class WeatherModel
    {
        public static WeatherModel FromJson(string json) => JsonConvert.DeserializeObject<WeatherModel>(json, ElClimaUDEO.Model.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this WeatherModel self) => JsonConvert.SerializeObject(self, ElClimaUDEO.Model.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new TextConverter(),
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class TextConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Text) || t == typeof(Text?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Partly Cloudy":
                    return Text.PartlyCloudy;
                case "Rain":
                    return Text.Rain;
                case "Scattered Thunderstorms":
                    return Text.ScatteredThunderstorms;
            }
            throw new Exception("Cannot unmarshal type Text");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Text)untypedValue;
            switch (value)
            {
                case Text.PartlyCloudy:
                    serializer.Serialize(writer, "Partly Cloudy"); return;
                case Text.Rain:
                    serializer.Serialize(writer, "Rain"); return;
                case Text.ScatteredThunderstorms:
                    serializer.Serialize(writer, "Scattered Thunderstorms"); return;
            }
            throw new Exception("Cannot marshal type Text");
        }
    }
}

