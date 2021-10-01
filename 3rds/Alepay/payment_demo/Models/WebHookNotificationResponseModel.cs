using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace payment_demo.Models
{
    public class WebHookNotificationResponseModel : alepay.Models.WebHookNotification
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string ReceivedDateStr { get; set; }
        public string ReceivedTimeStr { get; set; }
        public string RawContent { get; set; }
        public bool ValidChecksum { get; set; }
    }
}
