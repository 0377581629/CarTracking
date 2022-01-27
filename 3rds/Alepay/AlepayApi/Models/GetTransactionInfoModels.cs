using alepay.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace alepay.Models
{
    public class GetTransactionInfoRequestModel : RequestModel
    {
        [APIParam(Required = true)]
        public string TransactionCode { get; set; }
    }

    public class GetTransactionInfoResponseModel : ResponseModel
    {
        public string TransactionCode { get; set; }

        /// <summary>
        /// Mã đơn hàng của Merchan
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// Giá trị đơn hàng
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// Loại tiền tệ
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Email người mua hàng
        /// </summary>
        public string BuyerEmail { get; set; }

        /// <summary>
        /// Số điện thoại người mua
        /// </summary>
        public string BuyerPhone { get; set; }

        /// <summary>
        /// Thông tin thẻ khách hàng (6 số đầu và 4 số cuối)
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Tên người mua hàng
        /// </summary>
        public string BuyerName { get; set; }

        /// <summary>
        /// Tình trạng giao dịch
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Lý do thất bại (nếu có) 
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Mô tả trạng thái
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// True : Giao dịch trả góp, False: Giao dịch thường
        /// </summary>
        public bool Installment { get; set; }

        /// <summary>
        /// True : Thẻ 3D, False: Thẻ 2D
        /// </summary>
        public bool Is3D { get; set; }

        /// <summary>
        /// Số tháng trả góp
        /// </summary>
        public int? Month { get; set; }

        /// <summary>
        /// Mã ngân hàng trả góp
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// Tên ngân hàng trả góp
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Loại thẻ
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Thời gian thực hiện thanh toán (millisecond)
        /// </summary>
        public long? TransactionTime { get; set; }

        public DateTime? TransactionTimeWindows => AlePayApiClient.UnixTimeStampMilisecsToDateTime(TransactionTime);

        /// <summary>
        /// Thời gian thanh toán thành công (millisecond)
        /// </summary>
        public long? SuccessTime { get; set; }

        /// <summary>
        /// Số Hotline của ngân hàng trả góp
        /// </summary>
        public string BankHotline { get; set; }

        /// <summary>
        /// Phí merchant
        /// </summary>
        public double? MerchantFee { get; set; }

        /// <summary>
        /// Phí người thanh toán
        /// </summary>
        public double? PayerFee { get; set; }
    }
}
