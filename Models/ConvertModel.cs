using Newtonsoft.Json;

namespace telegram_spamer.Models
{
    public class ConvertModel
    {
        [JsonProperty("apikey")] public string ApiKey { get; set; }
        public string Input { get; set; }
        public string File { get; set; }
        [JsonProperty("filename")] public string FileName { get; set; }
        [JsonProperty("outputformat")] public string OutputFormat { get; set; }
        public object Options { get; set; }
    }
}