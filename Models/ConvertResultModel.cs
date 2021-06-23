using Newtonsoft.Json;

namespace telegram_spamer.Models
{
    public class ConvertLoadData
    {
        public string Id { get; set; }
        public int Minutes { get; set; }
    }

    public class ConvertStatusData
    {
        public string Id { get; set; }
        public string Step { get; set; }
        [JsonProperty("step_percent")] public string StepPercent { get; set; }
        public int Minutes { get; set; }
        public ConvertStatusOutput Output { get; set; }
    }

    public class ConvertStatusOutput
    {
        public string Url { get; set; }
        public string Size { get; set; }
    }

    public class ConvertStatusResult : BaseConvertResult<ConvertStatusData>
    {
    }

    public class ConvertLoadResult : BaseConvertResult<ConvertLoadData>
    {
    }

    public abstract class BaseConvertResult<T>
    {
        public int Code { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
    }
}