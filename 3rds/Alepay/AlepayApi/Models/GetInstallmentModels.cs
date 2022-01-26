using alepay.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace alepay.Models
{
    public class GetInstallmentRequestModel : RequestModel
    {
        /// <summary>
        /// Số tiền thanh toán trả góp
        /// </summary>
        [APIParam(Required = true)]
        public double Amount { get; set; }

        /// <summary>
        /// Mã tiền tệ
        /// </summary>
        [APIParam]
        public string CurrencyCode { get; set; }
    }

    public class GetInstallmentResponseModel : ResponseModel
    {
        public List<Plan> Data { get; set; }
        public class Plan
        {
            /// <summary>
            /// Mã ngân hàng trả góp
            /// </summary>
            public string BankCode { get; set; }

            /// <summary>
            /// Tên ngân hàng trả góp
            /// </summary>
            public string BankName { get; set; }

            public List<PaymentMethodInfo> PaymentMethods { get; set; }

            public class PaymentMethodInfo
            {
                /// <summary>
                /// Phương thức thanh toán
                /// </summary>
                public string PaymentMethod { get; set; }

                public List<Period> Periods { get; set; }
                public class Period
                {
                    /// <summary>
                    /// Kỳ hạn trả góp
                    /// </summary>
                    public int? Month { get; set; }

                    /// <summary>
                    /// Tổng phí trả góp người mua phải trả
                    /// amountFee = payerFlatFee + % payerPercentFee * amount +  payerInstallmentFlatFee + %payerInstallmentPercentFee* amount
                    /// </summary>
                    public double? AmountFee { get; set; }

                    /// <summary>
                    /// Tổng tiền khách hàng phải trả amountFinal = amount + amountFee
                    /// </summary>
                    public string AmountFinal { get; set; }

                    /// <summary>
                    /// Số tiền phải trả mỗi tháng
                    /// </summary>
                    public string AmountByMonth { get; set; }

                    /// <summary>
                    /// Phí cố định người mua phải trả
                    /// </summary>
                    public double? PayerFlatFee { get; set; }

                    /// <summary>
                    /// % phí cố định phải trả/giá trị đơn hàng
                    /// </summary>
                    public double? PayerPercentFee { get; set; }

                    /// <summary>
                    /// Phí chuyển đổi trả góp
                    /// </summary>
                    public double? PayerInstallmentFlatFee { get; set; }

                    /// <summary>
                    /// % phí chuyển đổi trả góp/giá trị đơn hàng
                    /// </summary>
                    public double? PayerInstallmentPercentFee { get; set; }

                    /// <summary>
                    /// Mã tiền tệ
                    /// </summary>
                    public string Currency { get; set; }
                }
            }
        }
    }
}
