using Newtonsoft.Json;

namespace GHN.Models
{
    public class RequestModel
    { }

    public class GetDistrictModel : RequestModel
    {
        [JsonProperty("province_id")]
        public int ProvinceId { get; set; }
    }
    
    public class GetWardModel : RequestModel
    {
        [JsonProperty("district_id")]
        public int DistrictId { get; set; }
    }
    
    public class CreateStoreModel: RequestModel
    {
        [JsonProperty("district_id")]
        public ulong DistrictId { get; set; }
        
        [JsonProperty("ward_code")]
        public string WardId { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("phone")]
        public string Phone { get; set; }
        
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
