using System;
using System.Collections.Generic;
using System.Text;

namespace alepay
{
    public class ResponseModel
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Signature { get; set; }
    }
}
