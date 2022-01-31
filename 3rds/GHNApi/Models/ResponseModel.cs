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

    #region Order
    public class SearchOrderResponseModel
    {
        [JsonProperty("data")]
        public List<OrderInfo> Orders { get; set; }
        
        [JsonProperty("total")]
        public ulong Total { get; set; }
    }

    public class CreateOrderResponseModel
    {
        [JsonProperty("expected_delivery_time")]
        public string ExpectedDeliveryTimeUTC { get; set; }
        
        [JsonProperty("fee")]
        public CreateOrderFeeResponseModel Fee { get; set; }
        
        [JsonProperty("order_code")]
        public string OrderCode { get; set; }
        
        [JsonProperty("sort_code")]
        public string SortCode { get; set; }
        
        [JsonProperty("total_fee")]
        public uint TotalFee { get; set; }
        
        [JsonProperty("trans_type")]
        public string TransType { get; set; }
    }

    public class CreateOrderFeeResponseModel
    {
        [JsonProperty("coupon")]
        public uint Coupon { get; set; }
        
        [JsonProperty("insurance")]
        public uint Insurance { get; set; }
        
        [JsonProperty("main_service")]
        public uint MainService { get; set; }
        
        [JsonProperty("r2s")]
        public uint ReturnToStore { get; set; }
        
        [JsonProperty("return")]
        public uint Return { get; set; }
        
        [JsonProperty("station_do")]
        public uint PickupFeeAtStation { get; set; }
        
        [JsonProperty("station_pu")]
        public uint DeliveryFeeAtStation { get; set; }
    }
    #endregion
    
    #region Fee
    public class FeeCalculationResponseModel
    {
        [JsonProperty("total")]
        public uint Total { get; set; }
        
        [JsonProperty("service_fee")]
        public uint Service { get; set; }
        
        [JsonProperty("insurance_fee")]
        public uint Insurance { get; set; }
        
        [JsonProperty("pick_station_fee")]
        public uint PickStation { get; set; }
        
        [JsonProperty("coupon_fee")]
        public uint Coupon { get; set; }
        
        [JsonProperty("r2s_fee")]
        public uint ReturnToStore { get; set; }
    }
    #endregion
}
