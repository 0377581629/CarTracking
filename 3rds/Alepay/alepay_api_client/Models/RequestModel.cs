using alepay.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace alepay
{
    public class RequestModel
    {
        /// <summary>
        /// Mã token key do alepay cung cấp khi đăng ký tài khoản trên alepay
        /// </summary>
        [APIParam(Required = true)]
        public virtual string TokenKey { get; set; }

        /// <summary>
        /// Chữ ký để kiểm tra thông tin
        /// </summary>
        // [APIParam(Required = true)]
        public virtual string Signature { get; set; }
    }
}
