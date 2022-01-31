using System.Collections.Generic;
using Zero.Extensions;

namespace GHN
{
    public class GHNDefs
    {
        public static Dictionary<string, string> ShippingStatuses = new Dictionary<string, string>
        {
            { "ready_to_pick" , "Shipping order has just been created"},
            { "picking", "Shipper is coming to pick up the goods"},
            { "cancel", "Shipping order has been cancelled"},
            { "money_collect_picking", "Shipper are interacting with the seller"},
            { "picked", "Shipper is picked the goods"},
            { "storing", "The goods has been shipped to GHN sorting hub"},
            { "transporting", "The goods are being rotated"},
            { "sorting", "The goods are being classified (at the warehouse classification)"},
            { "delivering", "Shipper is delivering the goods to customer"},
            { "money_collect_delivering", "Shipper is interacting with the buyer"},
            { "delivered", "The goods has been delivered to customer"},
            { "delivery_fail", "The goods hasn't been delivered to customer"},
            { "waiting_to_return", "The goods are pending delivery (can be delivered within 24/48h)"},
            { "return", "The goods are waiting to return to seller/merchant after 3 times delivery failed"},
            { "return_transporting", "The goods are being rotated"},
            { "return_sorting", "The goods are being classified (at the warehouse classification)"},
            { "returning", "The shipper is returning for seller"},
            { "return_fail", "The returning is failed"},
            { "returned", "The goods has been returned to seller/merchant"},
            { "exception", "The goods exception handling (cases that go against the process)."},
            { "damage", "Damaged goods"},
            { "lost", "The goods are lost"}
        };

        public enum RequiredNote
        {
            [StringValue("GHN_Delivery_AllowTest")] ChoThuHang,
            [StringValue("GHN_Delivery_AllowView")] ChoXemHangKhongThu,
            [StringValue("GHN_Delivery_NotView")] KhongChoXemHang
        }
    }
}