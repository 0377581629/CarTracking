using System.Collections.Generic;
using Newtonsoft.Json;

namespace GHN.Models
{
    public class RequestModel
    {
    }

    #region Address

    public class GetDistrictRequestModel : RequestModel
    {
        [JsonProperty("province_id")] public int ProvinceId { get; set; }
    }

    public class GetWardRequestModel : RequestModel
    {
        [JsonProperty("district_id")] public int DistrictId { get; set; }
    }

    #endregion

    #region Store

    public class CreateStoreRequestModel : RequestModel
    {
        [JsonProperty("district_id")] public ulong DistrictId { get; set; }

        [JsonProperty("ward_code")] public string WardId { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("phone")] public string Phone { get; set; }

        [JsonProperty("address")] public string Address { get; set; }
    }

    #endregion

    #region Order

    public class SearchOrderRequestModel : RequestModel
    {
        [JsonProperty("option_value")] public string Filter { get; set; }
        
        [JsonProperty("offset")] public ulong Offset { get; set; }

        [JsonProperty("limit")] public ulong Limit { get; set; } = 20;
        
        [JsonProperty("payment_type_id")] public List<int> PaymentTypes { get; set; }
        
        [JsonProperty("status")] public List<string> Statuses { get; set; }
    }

    public class CreateOrderRequestModel : RequestModel
    {
        [JsonProperty("client_order_code")]
        public string ClientOrderCode { get; set; }
        
        [JsonProperty("items")]
        public Item[] Items { get; set; }
        
        [JsonProperty("content")]
        public string Content { get; set; }
        
        /// <summary>
        /// Note for shipper
        /// </summary>
        [JsonProperty("note")]
        public string Note { get; set; }
        
        /// <summary>
        /// Accept values: CHOTHUHANG, CHOXEMHANGKHONGTHU, KHONGCHOXEMHANG
        /// </summary>
        [JsonProperty("required_note")]
        public string RequiredNote { get; set; }

        #region Payment
        [JsonProperty("coupon")]
        public string Coupon { get; set; }
        
        [JsonProperty("cod_amount")]
        public uint CODAmount { get; set; }
        
        [JsonProperty("payment_type_id")]
        public uint PaymentTypeId { get; set; }
        
        [JsonProperty("service_type_id")]
        public uint ServiceTypeId { get; set; }
        
        [JsonProperty("service_id")]
        public uint ServiceId { get; set; }
        
        [JsonProperty("insurance_value")]
        public uint InsuranceValue { get; set; }
        
        #endregion
        
        #region To
        [JsonProperty("to_name")]
        public string ToName { get; set; }
        
        [JsonProperty("to_phone")]
        public string ToPhone { get; set; }
        
        [JsonProperty("to_address")]
        public string ToAddress { get; set; }
        
        [JsonProperty("to_ward_code")]
        public string ToWardCode { get; set; }
        
        [JsonProperty("to_district_id")]
        public uint ToDistrictId { get; set; }
        #endregion
        
        #region Return
        
        [JsonProperty("return_phone")]
        public string ReturnPhone { get; set; }
        
        [JsonProperty("return_address")]
        public string ReturnAddress { get; set; }
        
        [JsonProperty("return_district_id")]
        public uint ReturnDistrictId { get; set; }
        
        [JsonProperty("return_ward_code")]
        public string ReturnWardCode { get; set; }
        
        #endregion
        
        #region Dimension
        [JsonProperty("weight")]
        public uint Weight { get; set; }
        
        [JsonProperty("length")]
        public uint Length { get; set; }
        
        [JsonProperty("width")]
        public uint Width { get; set; }
        
        [JsonProperty("height")]
        public uint Height { get; set; }
        #endregion
        
        #region Delivery
        /// <summary>
        /// The shipper not pickup parcels at shop’s address
        /// </summary>
        [JsonProperty("pick_station_id")]
        public uint PickStationId { get; set; }
        
        /// <summary>
        /// Picking shift
        /// </summary>
        [JsonProperty("pick_shift")]
        public int[] PickShift { get; set; }
        #endregion
    }
    
    #endregion
    
    #region Fee

    /// <summary>
    /// Service Type Id is null while ServiceId not null.
    /// </summary>
    public class FeeCalculationRequestModel : RequestModel
    {
        [JsonProperty("from_district_id")]
        public uint FromDistrictId { get; set; }
        
        [JsonProperty("to_district_id")]
        public uint ToDistrictId { get; set; }
        
        [JsonProperty("to_ward_code")]
        public string ToWardCode { get; set; }
        
        #region Dimension
        [JsonProperty("weight")]
        public uint Weight { get; set; }
        
        [JsonProperty("length")]
        public uint Length { get; set; }
        
        [JsonProperty("width")]
        public uint Width { get; set; }
        
        [JsonProperty("height")]
        public uint Height { get; set; }
        #endregion
        
        [JsonProperty("service_id")]
        public uint? ServiceId { get; set; }
        
        [JsonProperty("service_type_id")]
        public uint? ServiceTypeId { get; set; }
        
        [JsonProperty("insurance_value")]
        public uint InsuranceValue { get; set; }
        
        [JsonProperty("coupon")]
        public string Coupon { get; set; }
    }
    #endregion
    
    #region Station

    public class GetStationRequestModel : RequestModel
    {
        [JsonProperty("district_id")]
        public string DistrictId { get; set; }
        
        [JsonProperty("ward_code")]
        public string WardCode { get; set; }

        [JsonProperty("offset")]
        public uint Offset { get; set; } = 0;

        [JsonProperty("limit")]
        public uint Limit { get; set; } = 1000;
    }
    #endregion
}