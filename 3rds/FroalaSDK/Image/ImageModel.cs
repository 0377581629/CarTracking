using Newtonsoft.Json;

namespace FroalaEditor
{
    public class ImageModel
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("thumb")]
        public string Thumb { get; set; }
    }
}