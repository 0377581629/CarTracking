using System.Collections.Generic;
using Newtonsoft.Json;

namespace GHTK.Models
{
    #region Address

    public class Province
    {
        [JsonProperty("code")] public uint Id { get; set; }

        [JsonProperty("codename")] public string Code { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("division_type")] public string DivisionType { get; set; }

        [JsonProperty("phone_code")] public uint PhoneCode { get; set; }

        [JsonProperty("districts")] public List<District> Districts { get; set; }
    }

    public class District
    {
        [JsonProperty("code")] public uint Id { get; set; }

        [JsonProperty("codename")] public string Code { set; get; }

        [JsonProperty("short_codename")] public string ShortCode { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("division_type")] public string DivisionType { get; set; }

        public uint ProvinceId { set; get; }

        [JsonProperty("wards")] public List<Ward> Wards { get; set; }
    }

    public class Ward
    {
        [JsonProperty("code")] public uint Id { get; set; }

        [JsonProperty("codename")] public string Code { set; get; }

        [JsonProperty("short_codename")] public string ShortCode { get; set; }

        [JsonProperty("name")] public string Name { set; get; }

        [JsonProperty("division_type")] public string DivisionType { get; set; }

        public uint DistrictId { set; get; }
    }

    #endregion

    public class PickAddress
    {
        [JsonProperty("pick_address_id")] public uint Id { get; set; }

        [JsonProperty("address")] public string Address { get; set; }

        [JsonProperty("pick_tel")] public string Phone { get; set; }

        [JsonProperty("pick_name")] public string Name { get; set; }
    }

    public class Order
    {
        [JsonProperty("id")] public string Code { get; set; }

        [JsonProperty("note")] public string Note { get; set; }

        #region Pick

        [JsonProperty("pick_money")] public uint PickMoney { get; set; }

        [JsonProperty("pick_name")] public string PickName { get; set; }

        [JsonProperty("pick_address_id")] public string PickAddressId { get; set; }

        [JsonProperty("pick_province")] public string PickProvinceName { get; set; }

        [JsonProperty("pick_district")] public string PickDistrictName { get; set; }

        [JsonProperty("pick_ward")] public string PickWardName { get; set; }

        [JsonProperty("pick_street")] public string PickStreetName { get; set; }

        [JsonProperty("pick_address")] public string PickAddressDetail { get; set; }

        [JsonProperty("pick_tel")] public string PickPhone { get; set; }

        [JsonProperty("pick_email")] public string PickEmail { get; set; }

        #endregion

        #region Receive

        [JsonProperty("name")] public string ReceiveName { get; set; }

        [JsonProperty("province")] public string ReceiveProvinceName { get; set; }

        [JsonProperty("district")] public string ReceiveDistrictName { get; set; }

        [JsonProperty("ward")] public string ReceiveWardName { get; set; }

        [JsonProperty("street")] public string ReceiveStreetName { get; set; }

        [JsonProperty("hamlet")] public string ReceiveHamlet { get; set; } = "Khác";

        [JsonProperty("address")] public string ReceiveAddressDetail { get; set; }

        [JsonProperty("tel")] public string ReceivePhone { get; set; }

        [JsonProperty("email")] public string ReceiveEmail { get; set; }

        #endregion

        #region Return

        /// <summary>
        /// 0 or 1 - 0 Return address is Pick address, 1 to use diff address
        /// </summary>
        [JsonProperty("use_return_address")]
        public int ReturnAddressSameAsPick { get; set; } = 0;

        [JsonProperty("return_name")] public string ReturnName { get; set; }

        [JsonProperty("email")] public string ReturnAddressId { get; set; }

        [JsonProperty("return_province")] public string ReturnProvinceName { get; set; }

        [JsonProperty("return_district")] public string ReturnDistrictName { get; set; }

        [JsonProperty("return_ward")] public string ReturnWardName { get; set; }

        [JsonProperty("return_street")] public string ReturnStreetName { get; set; }

        [JsonProperty("return_address")] public string ReturnAddressDetail { get; set; }

        [JsonProperty("return_tel")] public string ReturnPhone { get; set; }

        [JsonProperty("return_email")] public string ReturnEmail { get; set; }

        #endregion
        
        /// <summary>
        /// 0 or 1
        /// </summary>
        [JsonProperty("is_freeship")]
        public uint FreeShip { get; set; } = 0;
        
        [JsonProperty("value")] public uint InsuranceValue { get; set; }
        
        [JsonProperty("pick_session")] public string PickSession { get; set; }

        [JsonProperty("pick_option")] public string PickOption { get; set; } = "cod";

        [JsonProperty("deliver_option")] public string DeliverOption { get; set; } = "xteam";
        
        [JsonProperty("products")] public List<Product> Products { get; set; }
        
        [JsonProperty("tags")] public List<int> Tags { get; set; }
    }

    public class Product
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("weight")]
        public double WeightInKg { get; set; }
        
        [JsonProperty("product_code")]
        public string ProductCode { get; set; }
        
        [JsonProperty("quantity")]
        public uint Quantity { get; set; }
    }

    public class OrderStatus
    {
        [JsonProperty("label_id")]
        public string LabelId { get; set; }
        
        [JsonProperty("partner_id")]
        public string PartnerId { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("status_text")]
        public string StatusText { get; set; }
        
        [JsonProperty("created")]
        public string CreationTime { get; set; }
        
        [JsonProperty("modified")]
        public string ModifiedTime { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("pick_date")]
        public string PickDate { get; set; }
        
        [JsonProperty("deliver_date")]
        public string DeliverDate { get; set; }
        
        [JsonProperty("customer_fullname")]
        public string ReceiveFullName { get; set; }
        
        [JsonProperty("customer_tel")]
        public string ReceivePhone { get; set; }
        
        [JsonProperty("address")]
        public string ReceiveAddress { get; set; }
        
        [JsonProperty("storage_day")]
        public uint StorageDays { get; set; }
        
        [JsonProperty("ship_money")]
        public uint ShipFee { get; set; }
        
        [JsonProperty("insurance")]
        public uint InsuranceFee { get; set; }
        
        [JsonProperty("value")]
        public uint InsuranceValue { get; set; }
        
        [JsonProperty("weight")]
        public uint WeightInGr { get; set; }
        
        [JsonProperty("pick_money")]
        public uint PickMoney { get; set; }
        
        [JsonProperty("is_freeship")]
        public uint IsFreeShip { get; set; }
    }

    public class PrintOrder : SimpleResponseModel
    {
        public byte[] LabelPdfFileBlob { get; set; }    
    }
}