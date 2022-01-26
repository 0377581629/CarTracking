using System;
using System.Collections.Generic;
using System.Text;
using alepay.Attributes;

namespace alepay.Models
{
    public class RequestPaymentRequestModel : RequestModel
    {
        /// <summary>
        /// Mã đơn hàng tại MerchantS
        /// </summary>
        [APIParam(Required = true)]
        public string OrderCode { get; set; }

        /// <summary>
        /// Mã đơn hàng của Merchant
        /// </summary>
        [APIParam(Required = true)]
        public string CustomMerchantId { get; set; }

        /// <summary>
        /// Giá trị đơn hàng
        /// </summary>
        [APIParam(Required = true)]
        public double Amount { get; set; }

        /// <summary>
        /// Loại tiền tệ (VND). Sử dụng hằng đã được định nghĩa 
        /// </summary>
        [APIParam(Required = true)]
        public string Currency { get; set; }

        /// <summary>
        /// Mô tả đơn hàng
        /// </summary>
        [APIParam(Required = true)]
        public string OrderDescription { get; set; }

        /// <summary>
        /// Tổng số sản phẩm trong đơn hàng
        /// </summary>
        [APIParam(Required = true)]
        public int TotalItem { get; set; }

        /// <summary>
        /// Kiểu thanh toán, sử dụng hằng được định nghĩa Definitions.CheckoutType
        /// </summary>
        [APIParam]
        public int CheckoutType { get; set; }

        /// <summary>
        /// True: Đơn hàng chỉ cho phép trả góp (
        /// Phải truyền lên cả month, bankCode và paymentMethod )
        /// False: Đơn hàng cho phép trả góp hoặc thanh toán thường
        /// </summary>
        [APIParam]
        public bool Installment { get; set; }

        /// <summary>
        /// Thông tin chu kỳ trả góp : 3,6,9,12,24 tháng
        /// </summary>
        [APIParam]
        public int? Month { get; set; }

        /// <summary>
        /// Mã ngân hàng cho phép User thực hiện thanh toán bằng ATM, IB, QRCODE
        /// (data có được sau khi gọi api): POST /get-list-banks
        /// Hoặc Mã ngân hàng cho phép User thực hiện thanh toán trả góp với thẻ quốc tế
        /// (data có được sau khi gọi api): POST /get-installment-info
        /// </summary>
        [APIParam]
        public string BankCode { get; set; }


        /// <summary>
        /// Các hình thức thanh toán, sử dụng các hằng đã được định nghĩa  Definitions.PM_*
        /// Trường hợp thanh toán trả góp paymentMethod là Loại thẻ cho phép
        /// user thực hiện thanh toán trả góp gồm VISA, MASTER, JCB
        /// </summary>
        [APIParam]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// URL Alepay sẽ callback lại Merchant khi user thanh toán thành công
        /// </summary>
        [APIParam(Required = true)]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// URL Alepay sẽ callback lại Merchant khi user từ chối thanh toán checkout
        /// </summary>
        [APIParam(Required = true)]
        public string CancelUrl { get; set; }

        /// <summary>
        /// Tên người mua hàng
        /// </summary>
        [APIParam(Required = true)]
        public string BuyerName { get; set; }

        /// <summary>
        /// Email người mua hàng
        /// </summary>
        [APIParam(Required = true)]
        public string BuyerEmail { get; set; }

        /// <summary>
        /// Số điện thoại người mua
        /// </summary>
        [APIParam(Required = true)]
        public string BuyerPhone { get; set; }

        /// <summary>
        /// Địa chỉ người mua
        /// </summary>
        [APIParam(Required = true)]
        public string BuyerAddress { get; set; }

        /// <summary>
        /// Tên thành phố của người mua
        /// </summary>
        [APIParam(Required = true)]
        public string BuyerCity { get; set; }

        /// <summary>
        /// Tên quốc gia của người mua
        /// </summary>
        [APIParam(Required = true)]
        public string BuyerCountry { get; set; }

        /// <summary>
        /// Thời gian cho phép thanh toán (tính bằng giờ)
        /// </summary>
        [APIParam]
        public string PaymentHours { get; set; }

        /// <summary>
        /// Mã chương trình khuyến mại
        /// </summary>
        [APIParam]
        public string PromotionCode { get; set; }

        /// <summary>
        /// True: Cho phép thanh toán bằng thẻ ATM/ IB hoặc QRCODE
        /// False: Cho phép chỉ thanh toán bằng thẻ quốc tế
        /// </summary>
        [APIParam]
        public bool AllowDomestic { get; set; }

        /// <summary>
        /// Ngôn ngữ hiển thị trên trang checkout.vi – tiếng Việt or en – tiếng Anh
        /// Sử dụng các hằng đã dược định nghĩa AlePayAPIClient.LANG_*
        /// </summary>
        [APIParam]
        public string Language { get; set; }
    }

    public class RequestPaymentResponse : ResponseModel
    {
        public string CheckoutUrl { get; set; }

        public string TransactionCode { get; set; }
    }
}
