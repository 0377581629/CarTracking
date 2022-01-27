using System.Collections.Generic;
using Newtonsoft.Json;

namespace GHN.Models
{
    public class ResponseModel<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("data")]
        public T Data { get; set; }
    }

    #region Store
    public class StoreResponseModel
    {
        [JsonProperty("last_offset")]
        public ulong LastOffset { get; set; }
        
        [JsonProperty("shops")]
        public List<Store> Stores { get; set; }
    }

    public class CreateStoreResponseModel
    {
        [JsonProperty("shop_id")]
        public ulong ShopId { get; set; }
    }
    
    #endregion
}
