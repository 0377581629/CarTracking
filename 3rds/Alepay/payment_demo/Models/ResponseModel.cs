using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace payment_demo.Models
{
    public class ResponseModel
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}
