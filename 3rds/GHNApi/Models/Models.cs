using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GHN.Models
{
    #region Address

    public class Province
    {
        [JsonProperty("ProvinceID")]
        public uint Id { get; set; }
        
        [JsonProperty("Code")]
        public string Code { get; set; }
        
        [JsonProperty("ProvinceName")]
        public string Name { get; set; }
    }
    
    [Serializable]
    public class District
    {
        [JsonProperty("DistrictID")]
        public uint Id { get; set; }
        
        [JsonProperty("ProvinceID")]
        public uint ProvinceId { set; get; }
        
        [JsonProperty("Code")]
        public string Code { set; get; }
        
        [JsonProperty("DistrictName")]
        public string Name { get; set; }
    }

    [Serializable]
    public class Ward
    {
        [JsonProperty("WardCode")]
        public string Code { set; get; }
        
        [JsonProperty("WardName")]
        public string Name { set; get; }
        
        [JsonProperty("DistrictID")]
        public uint DistrictId { set; get; }
    }
    
    #endregion
    
    #region Store

    public class Store
    {
        [JsonProperty("_id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("phone")]
        public string Phone { get; set; }
        
        [JsonProperty("address")]
        public string Address { get; set; }
        
        [JsonProperty("ward_code")]
        public string WardCode { get; set; }
        
        [JsonProperty("district_id")]
        public uint DistrictId { get; set; }
        
        [JsonProperty("client_id")]
        public uint ClientId { get; set; }
        
        [JsonProperty("bank_account_id")]
        public uint BankAccountId { get; set; }
        
        [JsonProperty("status")]
        public byte Status { get; set; }
        
        [JsonProperty("version_no")]
        public string Version { get; set; }
    }

    public class Station
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        
        [JsonProperty("locationId")]
        public uint LocationId { get; set; }
        
        [JsonProperty("locationCode")]
        public string LocationCode { get; set; }
        
        [JsonProperty("locationName")]
        public string LocationName { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        
        [JsonProperty("provinceName")]
        public string ProvinceName { get; set; }
        
        [JsonProperty("districtName")]
        public string DistrictName { get; set; }
        
        [JsonProperty("wardName")]
        public string WardName { get; set; }
        
        [JsonProperty("iframeMap")]
        public string IframeMap { get; set; }
        
    }
    
    #endregion
    
    #region Order
    
    public class OrderInfo
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        
        [JsonProperty("shop_id")]
        public uint ShopId { get; set; }
        
        [JsonProperty("client_id")]
        public uint ClientId { get; set; }
        
        [JsonProperty("client_order_code")]
        public string ClientOrderCode { get; set; }
        
        [JsonProperty("sort_code")]
        public string SortCode { get; set; }
        
        [JsonProperty("seal_code")]
        public string SealCode { get; set; }
        
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }

        [JsonProperty("order_code")]
        public string OrderCode { get; set; }
        
        [JsonProperty("order_date")]
        public string OrderDateUTC { get; set; }
        
        [JsonProperty("order_value")]
        public uint OrderValue { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }
        
        [JsonProperty("employee_note")]
        public string EmployeeNote { get; set; }
        
        #region Payment
        [JsonProperty("coupon")]
        public string Coupon { get; set; }
        
        [JsonProperty("insurance_value")]
        public uint InsuranceValue { get; set; }
        
        [JsonProperty("payment_type_id")]
        public int PaymentTypeId { get; set; }
        
        [JsonProperty("payment_type_ids")]
        public int[] PaymentTypeIds { get; set; }
        #endregion
        
        #region COD
        [JsonProperty("cod_collect_date")]
        public string CODCollectDateUTC { get; set; }
        
        [JsonProperty("cod_transfer_date")]
        public string CODTransferDate { get; set; }
        
        [JsonProperty("cod_amount")]
        public decimal CODAmount { get; set; }
        
        [JsonProperty("is_cod_collected")]
        public bool IsCODCollected { get; set; }
        
        [JsonProperty("is_cod_transferred")]
        public bool IsCODTransferred { get; set; }
        #endregion
        
        #region Dimension
        [JsonProperty("height")]
        public uint Height { get; set; }
        
        [JsonProperty("width")]
        public uint Width { get; set; }
        
        [JsonProperty("weight")]
        public uint Weight { get; set; }
        
        [JsonProperty("converted_weight")]
        public uint ConvertedWeight { get; set; }
        
        [JsonProperty("length")]
        public uint Length { get; set; }
        #endregion
        
        #region Current
        [JsonProperty("current_transport_warehouse_id")]
        public ulong CurrentTransportWarehouseId { get; set; }
        
        [JsonProperty("current_warehouse_id")]
        public ulong CurrentWarehouseId { get; set; }
        #endregion
        
        #region Service
        [JsonProperty("service_id")]
        public uint ServiceId { get; set; }
        
        [JsonProperty("service_type_id")]
        public uint ServiceTypeId { get; set; }
        
        [JsonProperty("custom_service_fee")]
        public decimal CustomServiceFee { get; set; }
        #endregion
        
        #region Delivery
        [JsonProperty("deliver_station_id")]
        public uint DeliverStationId { get; set; }
        
        [JsonProperty("deliver_warehouse_id")]
        public uint DeliverWarehouseId { get; set; }
        
        [JsonProperty("next_warehouse_id")]
        public uint NextWarehouseId { get; set; }
        
        [JsonProperty("leadtime")]
        public string ExpectedDeliveryTimeUTC { get; set; }
        
        [JsonProperty("finish_date")]
        public string FinishDateUTC { get; set; }
        
        [JsonProperty("is_partial_return")]
        public bool IsPartialReturn { get; set; }
        #endregion
        
        #region From
        [JsonProperty("from_address")]
        public string FromAddress { get; set; }
        
        [JsonProperty("from_district_id")]
        public uint FromDistrictId { get; set; }
        
        [JsonProperty("from_location")]
        public Location FromLocation { get; set; }
        
        [JsonProperty("from_name")]
        public string FromName { get; set; }
        
        [JsonProperty("from_phone")]
        public string FromPhone { get; set; }
        
        [JsonProperty("from_ward_code")]
        public string FromWardCode { get; set; }
        #endregion
        
        #region To
        [JsonProperty("to_address")]
        public string ToAddress { get; set; }
        
        [JsonProperty("to_district_id")]
        public uint ToDistrictId { get; set; }
        
        [JsonProperty("to_location")]
        public Location ToLocation { get; set; }
        
        [JsonProperty("to_name")]
        public string ToName { get; set; }
        
        [JsonProperty("to_phone")]
        public string ToPhone { get; set; }
        
        [JsonProperty("to_ward_code")]
        public string ToWardCode { get; set; }
        #endregion
        
        #region Pick
        [JsonProperty("pick_station_id")]
        public uint PickStationId { get; set; }
        
        [JsonProperty("pick_warehouse_id")]
        public uint PickWarehouseId { get; set; }
        
        [JsonProperty("pickup_time")]
        public string PickUpTimeUTC { get; set; }
        #endregion
        
        #region Return
        [JsonProperty("required_note")]
        public string RequiredNote { get; set; }
        
        [JsonProperty("return_address")]
        public string ReturnAddress { get; set; }
        
        [JsonProperty("return_district_id")]
        public ulong ReturnDistrictId { get; set; }
        
        [JsonProperty("return_location")]
        public Location ReturnLocation { get; set; }
        
        [JsonProperty("return_name")]
        public string ReturnName { get; set; }
        
        [JsonProperty("return_phone")]
        public string ReturnPhone { get; set; }
        
        [JsonProperty("return_ward_code")]
        public string ReturnWardCode { get; set; }
        
        [JsonProperty("return_warehouse_id")]
        public uint ReturnWarehouseId { get; set; }
        #endregion

        #region Created Info
        [JsonProperty("created_client")]
        public uint CreatedClient { get; set; }
        
        [JsonProperty("created_date")]
        public string CreatedDateUTC { get; set; }
        
        [JsonProperty("created_employee")]
        public uint CreatedEmployee { get; set; }
        
        [JsonProperty("created_ip")]
        public string CreatedIp { get; set; }
        
        [JsonProperty("created_source")]
        public string CreatedSource { get; set; }
        #endregion
        
        #region UpdatedInfo
        [JsonProperty("updated_client")]
        public uint UpdatedClient { get; set; }
        
        [JsonProperty("updated_date")]
        public string UpdatedDateUTC { get; set; }
        
        [JsonProperty("updated_employee")]
        public uint UpdatedEmployee { get; set; }
        
        [JsonProperty("updated_ip")]
        public string UpdatedIp { get; set; }
        
        [JsonProperty("updated_source")]
        public string UpdatedSource { get; set; }
        
        [JsonProperty("updated_warehouse")]
        public uint UpdatedWarehouse { get; set; }
        #endregion

        [JsonProperty("soc_id")]
        public string SOCId { get; set; }
        
        [JsonProperty("log")]
        public Log[] Logs { get; set; }
        
        [JsonProperty("tag")]
        public string[] Tags { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("version_no")]
        public string Version { get; set; }
    }

    public class Location
    {
        [JsonProperty("cell_code")]
        public string CellCode { get; set; }
        
        [JsonProperty("lat")]
        public double Lat { get; set; }
        
        [JsonProperty("long")]
        public double Long { get; set; }
        
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }
        
        [JsonProperty("trust_level")]
        public byte TrustLevel { get; set; }
        
        [JsonProperty("wardcode")]
        public string WardCode { get; set; }
    }

    public class Item
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("quantity")]
        public uint Quantity { get; set; }
    }

    public class Log
    {
        [JsonProperty("payment_type_id")]
        public uint PaymentTypeId { get; set; }
        
        [JsonProperty("driver_id")]
        public uint DriverId { get; set; }
        
        [JsonProperty("driver_name")]
        public string DriverName { get; set; }
        
        [JsonProperty("reason_code")]
        public string ReasonCode { get; set; }
        
        [JsonProperty("reason")]
        public string  Reason { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("updated_date")]
        public string UpdatedDateUTC { get; set; }
    }
    
    #endregion

    #region Pick Shift
    public class PickShift
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("from_time")]
        public uint FromTime { get; set; }
        
        [JsonProperty("to_time")]
        public uint ToTime { get; set; }
    }
    #endregion
    
    #region Services

    public class Service
    {
        [JsonProperty("service_id")]
        public uint Id { get; set; }
        
        [JsonProperty("service_type_id")]
        public uint TypeId { get; set; }
        
        [JsonProperty("short_name")]
        public string Name { get; set; }
    }
    #endregion
    
    [Serializable]
    public class OrderCost
    {
        public decimal Cost { set; get; }
        public string Name { set; get; }
        public decimal ServiceId { set; get; }
    }
}