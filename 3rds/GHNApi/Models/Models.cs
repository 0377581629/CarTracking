using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GHN.Models
{
    #region Address

    public class Province
    {
        [JsonProperty("ProvinceID")]
        public ulong Id { get; set; }
        
        [JsonProperty("Code")]
        public string Code { get; set; }
        
        [JsonProperty("ProvinceName")]
        public string Name { get; set; }
    }
    
    [Serializable]
    public class District
    {
        [JsonProperty("DistrictID")]
        public ulong Id { get; set; }
        
        [JsonProperty("ProvinceID")]
        public ulong ProvinceId { set; get; }
        
        [JsonProperty("Code")]
        public string Code { set; get; }
        
        [JsonProperty("DistrictName")]
        public string Name { get; set; }
    }

    [Serializable]
    public class Ward
    {
        [JsonProperty("WardCode")]
        public ulong Id { set; get; }
        
        [JsonProperty("WardName")]
        public string Name { set; get; }
        
        [JsonProperty("DistrictID")]
        public ulong DistrictId { set; get; }
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
        public ulong DistrictId { get; set; }
        
        [JsonProperty("client_id")]
        public ulong ClientId { get; set; }
        
        [JsonProperty("bank_account_id")]
        public ulong BankAccountId { get; set; }
        
        [JsonProperty("status")]
        public byte Status { get; set; }
        
        [JsonProperty("version_no")]
        public string Version { get; set; }
    }

    
    #endregion
    [Serializable]
    public class HubUpdateRequest
    {
        public string token { set; get; }
        public decimal HubID { set; get; }
        public string Address { set; get; }
        public string ContactName { set; get; }
        public string ContactPhone { set; get; }
        public decimal DistrictID { set; get; }
        public string Email { set; get; }
        public bool IsMain { set; get; }
        public decimal Latitude { set; get; }
        public decimal Longitude { set; get; }
    }

    [Serializable]
    public class HubAddRequest
    {
        public string token {set;get;}
        public decimal HubID {set;get;}
        public string Address {set;get;}
        public string ContactName {set;get;}
        public string ContactPhone {set;get;}
        public decimal DistrictID {set;get;}
        public string Email {set;get;}
        public bool IsMain {set;get;}
        public decimal Latitude {set;get;}
        public decimal Longitude {set;get;}
    }

    

    [Serializable]
    public class LoginRequest
    {
        public string token { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
    }

    [Serializable]
    public class RegisterRequest
    {
        public string token { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string ContactPhone { set; get; }
        public string ContactName { set; get; }
    }

    [Serializable]
    public class ClientInfo
    {
        public decimal ClientID { set; get; }
        public string ClientName { set; get; }
        public string Token { set; get; }
    }

    [Serializable]
    public class LoginResponse
    {
        public decimal code { set; get; }
        public string msg { set; get; }
        public ClientInfo data { set; get; }
    }

    [Serializable]
    public class OrderLogsRequest
    {
        public string token { set; get; }
        public decimal FromTime { set; get; }
        public decimal ToTime { set; get; }
        public Condition Condition { set; get; }
        public decimal Skip { set; get; }
    }

    [Serializable]
    public class Condition
    {
        public decimal CustomerID { set; get; }
        public decimal ShippingOrderID { set; get; }
        public decimal OrderCode { set; get; }
        public string CurrentStatus { set; get; }
    }

    [Serializable]
    public class Log
    {
        public string OrderCode { set; get; }
        public decimal ShippingOrderID { set; get; }
        public string CurrentStatus { set; get; }
        public decimal CustomerID { set; get; }
        public bool IsPushed { set; get; }
        public OrderLogInfo OrderInfo { set; get; }
        public decimal StatusCode { set; get; }
        public string CreateTime { set; get; }
        public string UpdateTime { set; get; }
    }

    [Serializable]
    public class OrderLogInfo
    {
        public decimal CoDAmount { set; get; }
        public string CurrentStatus { set; get; }
        public string CurrentWarehouseName { set; get; }
        public decimal CustomerID { set; get; }
        public string CustomerName { set; get; }
        public string CustomerPhone { set; get; }
        public string ExternalCode { set; get; }
        public string Note { set; get; }
        public decimal OrderCode { set; get; }
        public string ReturnInfo { set; get; }
        public string ServiceName { set; get; }
        public List<ShippingOrderCost> ShippingOrderCosts { set; get; }
        public decimal weight { set; get; }
    }

    [Serializable]
    public class ShippingOrderCost
    {
        public decimal Cost { set; get; }
        public decimal PaymentChannelID { set; get; }
        public decimal ServiceID { set; get; }
        public string ServiceName { set; get; }
        public decimal ServiceType { set; get; }
        public decimal ShippingOrderID { set; get; }
    }

    [Serializable]
    public class SetConfigRequest
    {
        public string token { set; get; }
        public List<string> TokenClient { set; get; }
        public bool ConfigCod { set; get; }
        public bool ConfigReturnData { set; get; }
        public string URLCallback { set; get; }
        public ConfigField ConfigField { set; get; }
        public ConfigStatus ConfigStatus { set; get; }
    }

    [Serializable]
    public class ConfigField
    {
        public bool CoDAmount { set; get; }
        public bool CurrentWarehouseName { set; get; }
        public bool CustomerID { set; get; }
        public bool CustomerName { set; get; }
        public bool CustomerPhone { set; get; }
        public bool Note { set; get; }
        public bool OrderCode { set; get; }
        public bool ServiceName { set; get; }
        public bool ShippingOrderCosts { set; get; }
        public bool Weight { set; get; }
        public bool ExternalCode { set; get; }
        public bool ReturnInfo { set; get; }
    }

    [Serializable]
    public class ConfigStatus
    {
        public bool ReadyToPick { set; get; }
        public bool Picking { set; get; }
        public bool Storing { set; get; }
        public bool Delivering { set; get; }
        public bool Delivered { set; get; }
        public bool WaitingToFinish { set; get; }
        public bool Return { set; get; }
        public bool Returned { set; get; }
        public bool Finish { set; get; }
        public bool LostOrder { set; get; }
        public bool Cancel { set; get; }
    }

    [Serializable]
    public class OrderUpdateRequest
    { 
        public string token {set; get;}
        public decimal ShippingOrderID {set; get;}
        public string OrderCode {set; get;}
        public decimal PaymentTypeID {set; get;}
        public decimal FromDistrictID {set; get;}
        public string FromWardCode {set; get;}
        public decimal ToDistrictID {set; get;}
        public string ToWardCode {set; get;}
        public string Note {set; get;}
        public string NoteCode {set; get;}
        public string ClientContactName {set; get;}
        public string ClientContactPhone {set; get;}
        public string ClientAddress {set; get;}
        public string CustomerName {set; get;}
        public string CustomerPhone {set; get;}
        public string ShippingAddress {set; get;}
        public decimal CoDAmount {set; get;}
        public decimal InsuranceFee {set; get;}
        public decimal ClientHubID {set; get;}
        public decimal ServiceID {set; get;}
        public string Content {set; get;}
        public string CouponCode {set; get;}
        public decimal Weight {set; get;}
        public decimal Length {set; get;}
        public decimal Width {set; get;}
        public decimal Height {set; get;}
        public string ReturnContactName {set; get;}
        public string ReturnContactPhone {set; get;}
        public string ReturnAddress {set; get;}
        public string ReturnDistrictCode {set; get;}
        public string ExternalReturnCode {set; get;}
        public List<OrderCostCal> OrderCosts {set; get;}
    }

    [Serializable]
    public class OrderUpdateResponse
    { 
        public string ErrorMessage {set; get;}
        public decimal OrderID {set; get;}
        public decimal PaymentTypeID {set; get;}
        public decimal CurrentStatus {set; get;}
        public decimal TotalServiceFee {set; get;}
        public decimal ClientHubID {set; get;}
        public string SortCode {set; get;}
    }

    [Serializable]
    public class OrderInfo
    {
        public string CODTransferDate { set; get; }
        public decimal CSLostPackageID { set; get; }
        public bool CheckMainBankAccount { set; get; }
        public decimal ClientHubID { set; get; }
        public decimal CoDAmount { set; get; }
        public string Content { set; get; }
        public string CouponCode { set; get; }
        public string CurrentStatus { set; get; }
        public string CurrentWarehouseName { set; get; }
        public decimal CustomerID { set; get; }
        public string CustomerName { set; get; }
        public string CustomerPhone { set; get; }
        public string DeliverWarehouseName { set; get; }
        public string EndDeliveryTime { set; get; }
        public string EndPickTime { set; get; }
        public string EndReturnTime { set; get; }
        public string ErrorMessage { set; get; }
        public string ExpectedDeliveryTime { set; get; }
        public string ExternalCode { set; get; }
        public string ExternalReturnCode { set; get; }
        public decimal ExtraFees { set; get; }
        public string FirstDeliveredTime { set; get; }
        public decimal FromDistrictID { set; get; }
        public decimal FromLat { set; get; }
        public decimal FromLng { set; get; }
        public string FromWardCode { set; get; }
        public decimal Height { set; get; }
        public decimal InsuranceFee { set; get; }
        public decimal Length { set; get; }
        public string Note { set; get; }
        public string NoteCode { set; get; }
        public decimal NumDeliver { set; get; }
        public decimal NumPick { set; get; }
        public decimal NumReturn { set; get; }
        public string OrderCode { set; get; }
        public decimal OriginPackageValue { set; get; }
        public string OriginServiceName { set; get; }
        public decimal PaymentTypeID { set; get; }
        public string PickAddress { set; get; }
        public string PickWarehouseName { set; get; }
        public string ReturnInfo { set; get; }
        public string SealCode { set; get; }
        public string SenderName { set; get; }
        public string SenderPhone { set; get; }
        public decimal ServiceID { set; get; }
        public string ServiceName { set; get; }
        public string ShippingAddress { set; get; }
        public decimal ShippingOrderID { set; get; }
        public string StartDeliveryTime { set; get; }
        public string StartPickTime { set; get; }
        public string StartReturnTime { set; get; }
        public decimal ToDistrictID { set; get; }
        public decimal ToLatitude { set; get; }
        public decimal ToLongitude { set; get; }
        public string ToWardCode { set; get; }
        public decimal TotalServiceCost { set; get; }
        public decimal Weight { set; get; }
        public decimal Width { set; get; }
        public string SortCode { set; get; }
        public List<ShippingOrderCost> ShippingOrderCosts { set; get; }
    }

    [Serializable]
    public class OrderCreateRequest
    {
        public string token { set; get; }
        public decimal PaymentTypeID { set; get; }
        public decimal FromDistrictID { set; get; }
        public string FromWardCode { set; get; }
        public decimal ToDistrictID { set; get; }
        public string ToWardCode { set; get; }
        public string Note { set; get; }
        public string SealCode { set; get; }
        public string ExternalCode { set; get; }
        public string ClientContactName { set; get; }
        public string ClientContactPhone { set; get; }
        public string ClientAddress { set; get; }
        public string CustomerName { set; get; }
        public string CustomerPhone { set; get; }
        public string ShippingAddress { set; get; }
        public decimal CoDAmount { set; get; }
        public string NoteCode { set; get; }
        public decimal InsuranceFee { set; get; }
        public decimal ClientHubID { set; get; }
        public decimal ServiceID { set; get; }
        public decimal ToLatitude { set; get; }
        public decimal ToLongitude { set; get; }
        public decimal FromLat { set; get; }
        public decimal FromLng { set; get; }
        public string Content { set; get; }
        public decimal Weight { set; get; }
        public decimal Length { set; get; }
        public decimal Width { set; get; }
        public decimal Height { set; get; }
        public bool CheckMainBankAccount { set; get; }
        public string ReturnContactName { set; get; }
        public string ReturnContactPhone { set; get; }
        public string ReturnAddress { set; get; }
        public decimal ReturnDistrictID { set; get; }
        public string ExternalReturnCode { set; get; }
        public bool IsCreditCreate { set; get; }
        public decimal AffiliateID { set; get; }
        List<String> ShippingOrderCosts { set; get; }
    }

    [Serializable]
    public class OrderCreateResponse
    {
        public string ErrorMessage {set; get;}
        public decimal OrderID {set; get;}
        public decimal PaymentTypeID {set; get;}
        public string OrderCode {set; get;}
        public decimal ExtraFee {set; get;}
        public decimal TotalServiceFee {set; get;}
        public string ExpectedDeliveryTime {set; get;}
        public decimal ClientHubID {set; get;}
        public string SortCode {set; get;}
    }

    [Serializable]
    public class AvailableServicesRequest
    {
        public string token { set; get; }
        public decimal Weight { set; get; }
        public decimal Length { set; get; }
        public decimal Width { set; get; }
        public decimal Height { set; get; }
        public decimal FromDistrictID { set; get; }
        public decimal ToDistrictID { set; get; }
    }

    [Serializable]
    public class AvailableServices
    {
        public string ExpectedDeliveryTime { set; get; }
        public string Name { set; get; }
        public decimal ServiceFee { set; get; }
        public decimal ServiceID { set; get; }
        public List<Extra> Extras { set; get; }
    }

    [Serializable]
    public class Extra
    {
        public decimal MaxValue { set; get; }
        public string Name { set; get; }
        public decimal ServiceFee { set; get; }
        public decimal ServiceID { set; get; }
    }

    
    
    [Serializable]
    public class Hub
    {
        public string Address { set; get; }
        public decimal ClientId { set; get; }
        public string ContactName { set; get; }
        public string ContactPhone { set; get; }
        public decimal DistrictId { set; get; }
        public string DistrictName { set; get; }
        public string Email { set; get; }
        public string FullAddress { set; get; }
        public decimal HubId { set; get; }
        public bool IsEditHub { set; get; }
        public bool IsMain { set; get; }
        public decimal Latitude { set; get; }
        public decimal Longitude { set; get; }
        public decimal ProvinceId { set; get; }
        public string ProvinceName { set; get; }
    }

    [Serializable]
    public class HubRequest
    {
        public string token { set; get; }
        public decimal HubID { set; get; }
        public string Address { set; get; }
        public string ContactName { set; get; }
        public string ContactPhone { set; get; }
        public decimal DistrictID { set; get; }
        public bool IsMain { set; get; }
        public decimal Latitude { set; get; }
        public decimal Longitude { set; get; }
    }

    [Serializable]
    public class CalculateFeeRequest
    {
        public string Token { set; get; }
        public decimal Weight { set; get; }
        public decimal Length { set; get; }
        public decimal Width { set; get; }
        public decimal Height { set; get; }
        public decimal FromDistrictId { set; get; }
        public decimal ToDistrictId { set; get; }
        public decimal ServiceId { set; get; }
        public string CouponCode { set; get; }
        public decimal InsuranceFee { set; get; }
        public List<OrderCostCal> OrderCosts { set; get; }
    }

    [Serializable]
    public class OrderCostCal
    {
        public decimal ServiceId { set; get; }
    }

    [Serializable]
    public class FeeResponse
    {
        public string ErrorMessage { set; get; }
        public decimal CalculatedFee { set; get; }
        public decimal ServiceFee { set; get; }
        public decimal CoDFee { set; get; }
        public decimal DiscountFee { set; get; }
        public decimal WeightDimension { set; get; }
        public List<OrderCost> OrderCosts { set; get; }
    }

    [Serializable]
    public class OrderCost
    {
        public decimal Cost { set; get; }
        public string Name { set; get; }
        public decimal ServiceId { set; get; }
    }
}