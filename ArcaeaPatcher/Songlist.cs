using Newtonsoft.Json;

namespace ArcaeaPatcher
{
    // 自定义 JsonConverter 用于处理 int 字段遇到 float 值的情况
    public class IntFromFloatConverter : JsonConverter<int>
    {
        public override int ReadJson(JsonReader reader, Type objectType, int existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Float)
            {
                // 若遇到 float 类型的值，将其转换为 int 类型
                float floatValue = Convert.ToSingle(reader.Value);
                return (int)floatValue;
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                // 若遇到 int 类型的值，直接返回
                return Convert.ToInt32(reader.Value);
            }
            else
            {
                // 若遇到其他类型的值，抛出异常
                throw new JsonSerializationException($"Unexpected token {reader.TokenType} when trying to deserialize an int.");
            }
        }

        public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer)
        {
            // 序列化时，直接写入 int 值
            writer.WriteValue(value);
        }
    }

    public class SongsData
    {
        [JsonProperty("songs")]
        public List<Song> Songs { get; set; }
    }

    public class TitleLocalized
    {
        [JsonProperty("en")]
        public string En { get; set; }
        [JsonProperty("ja")]
        public string Ja { get; set; }
        [JsonProperty("ko")]
        public string Ko { get; set; }
        [JsonProperty("zh-Hans")]
        public string Zh_Hans { get; set; }
        [JsonProperty("zh-Hant")]
        public string Zh_Hant { get; set; }
    }

    public class Difficulty
    {
        [JsonProperty("ratingClass")]
        public int RatingClass { get; set; }
        [JsonProperty("chartDesigner")]
        public string ChartDesigner { get; set; }
        [JsonProperty("jacketDesigner")]
        public string JacketDesigner { get; set; }
        [JsonProperty("rating")]
        public int Rating { get; set; }
        [JsonProperty("ratingPlus")]
        public bool RatingPlus { get; set; }
    }

    public class Song
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title_localized")]
        public TitleLocalized TitleLocalized { get; set; }
        [JsonProperty("artist")]
        public string Artist { get; set; }
        [JsonProperty("bpm")]
        public string Bpm { get; set; }
        [JsonProperty("bpm_base")]
        [JsonConverter(typeof(IntFromFloatConverter))]
        public int BpmBase { get; set; }
        [JsonProperty("set")]
        public string Set { get; set; }
        [JsonProperty("purchase")]
        public string Purchase { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("audioPreview")]
        public float AudioPreview { get; set; }
        [JsonProperty("audioPreviewEnd")]
        public float AudioPreviewEnd { get; set; }
        [JsonProperty("side")]
        public int Side { get; set; }
        [JsonProperty("bg")]
        public string Bg { get; set; }
        [JsonProperty("bg_inverse")]
        public string BgInverse { get; set; }
        [JsonProperty("world_unlock")]
        public bool WorldUnlock { get; set; }
        [JsonProperty("date")]
        public long Date { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("source_copyright")]
        public string SourceCopyright { get; set; }
        [JsonProperty("difficulties")]
        public List<Difficulty> Difficulties { get; set; }
        // 新增的文本属性
        [JsonIgnore]
        public string TitleLocalizedText { get; set; }
        [JsonIgnore]
        public string DifficultiesText { get; set; }
    }
}
