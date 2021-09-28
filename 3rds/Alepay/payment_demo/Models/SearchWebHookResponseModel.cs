using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace payment_demo.Models
{
    public class SearchWebHookResponseModel : ResponseModel
    {
        public List<Webhook> Data { get; set; }

        public class Webhook
        {
            public string Id { get; set; }

            public string TransactionCode { get; set; }

            public DateTime ReceivedTime { get; set; }

            public string ReceivedDateStr { get; set; }

            public string ReceivedTimeStr { get; set; }
        }
    }
}
