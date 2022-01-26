using System;
using System.Collections.Generic;
using System.Text;

namespace alepay.Models
{
    public class GetListBanksResponseModel : ResponseModel
    {
        public List<Bank> Data { get; set; }

        public class Bank
        {
            /// <summary>
            /// Mã ngân hàng
            /// </summary>
            public string BankCode { get; set; }

            /// <summary>
            /// Tên ngân hàng cho phép thanh toán
            /// </summary>
            public string BankFullName { get; set; }

            /// <summary>
            /// Phương thức thanh toán
            /// - ATM_ON: thanh toán bằng thẻ ATM
            /// - IB_ON: thanh toán bằng tài khoản IB
            /// QRCODE: thanh toán bằng cách quét mã QRCODE
            /// </summary>
            public string MethodCode { get; set; }

            public string UrlBankLogo { get; set; }
        }
    }
}
