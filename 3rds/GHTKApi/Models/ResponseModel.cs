using Newtonsoft.Json;

namespace GHTK.Models
{
    public class SimpleResponseModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
    }
    
    public class ResponseModel<T> : SimpleResponseModel
    {
        [JsonProperty("data")]
        public T Data { get; set; }
        
        [JsonProperty("order")]
        public T Order { get; set; }
    }

    public class OrderResponseModel
    {
        [JsonProperty("partner_id")]
        public string PartnerId { get; set; }
        
        [JsonProperty("label")]
        public string Label { get; set; }
        
        [JsonProperty("area")]
        public string Area { get; set; }
        
        [JsonProperty("fee")]
        public uint Fee { get; set; }
        
        [JsonProperty("insurance_fee")]
        public uint InsuranceFee { get; set; }
        
        [JsonProperty("estimated_pick_time")]
        public string EstimatePickTime { get; set; }
        
        [JsonProperty("estimated_deliver_time")]
        public string EstimateDeliverTime { get; set; }
        
        [JsonProperty("status_id")]
        public uint StatusId { get; set; }
    }
}
