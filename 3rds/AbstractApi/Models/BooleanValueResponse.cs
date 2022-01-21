using Newtonsoft.Json;

namespace AbstractApi.Models
{
    public class BooleanValueResponse
    {
        [JsonProperty("value")]
        public bool Value { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}