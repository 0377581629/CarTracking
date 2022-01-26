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
}
