using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace payment_demo.Models
{
    public class AlePayReturnModel
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string TransactionCode { get; set; }

        public bool UserCancelled { get; set; }
    }
}
