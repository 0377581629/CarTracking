using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using payment_demo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using alepay;
using alepay.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Routing;
using System.IO;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace payment_demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        AlePayAPIClient alepayClient = new AlePayAPIClient();

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> RequestPayment(RequestPaymentRequestModel model)
        {
            model.ReturnUrl = Url.Action("Return", "Home", null, Request.Scheme);
            model.CancelUrl = Url.Action("Cancel", "Home", null, Request.Scheme);
            model.CheckoutType = AlePayDefs.CO_TYPE_INSTANT_PAYMENT_WITH_ATM_IB_QRCODE_INTL_CARDS;
            model.Installment = false;
            model.AllowDomestic = true;
            var result = await alepayClient.RequestPaymentAsync(model);
            return Json(result);
        }

        public IActionResult Return(string errorCode, string transactionCode, string cancel)
        {
            int errorCodeInt = 999;
            int.TryParse(errorCode, out errorCodeInt);
            string message = AlePayAPIClient.GetErrorMessageForCode(errorCodeInt);
            bool userCancelled = false;
            bool.TryParse(cancel, out userCancelled);
            var model = new Models.AlePayReturnModel()
            {
                Code = errorCodeInt,
                Message = message,
                TransactionCode = transactionCode,
                UserCancelled = userCancelled
            };
            return View(model);
        }

        public IActionResult Cancel()
        {
            return View();
        }

        public IActionResult TransactionInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> QueryTransactionInfo(
            GetTransactionInfoRequestModel model)
        {
            var result = await alepayClient.GetTransactionInfoAsync(model);
            return Json(result);
        }

        public async Task<IActionResult> Banks()
        {
            var banks = await alepayClient.GetListBanksAsync();
            return View(banks);
        }

        public IActionResult Webhooks()
        {
            return View();
        }

        private const string WebHooksBasePath = "webhooks";

        [HttpPost]
        public async Task<JsonResult> SearchWebHooks(string keyword)
        {
            if (!Directory.Exists(WebHooksBasePath))
                return Json(new SearchWebHookResponseModel {
                    Success = false,
                    ErrorMessage = "Thư mục dữ liệu không tồn tại"
                });
            IEnumerable<string> files = null;
            try
            {
                string searchPattern = string.Format("{0}*.json", keyword);
                files = Directory.EnumerateFiles(WebHooksBasePath, searchPattern);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Không thể tìm kiếm webhook. Mã giao dịch: {0}", keyword);
                return Json(new SearchWebHookResponseModel
                {
                    Success = false,
                    ErrorMessage = "Lỗi không duyệt được danh sách file trong thư mục dữ liệu"
                });
            }
            var webHooks = new List<SearchWebHookResponseModel.Webhook>();
            foreach (var file in files)
            {
                string jsonContent = string.Empty;
                try
                {
                    jsonContent = await System.IO.File.ReadAllTextAsync(file);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Không thể đọc tệp tin webhook {0}", file);
                    continue;
                }
                var webHook = JsonConvert.DeserializeObject<WebHookNotification>(jsonContent);
                FileInfo fileInfo = new FileInfo(file);
                if (webHook.TransactionInfo == null)
                    continue;
                webHooks.Add(new SearchWebHookResponseModel.Webhook() {
                    TransactionCode = webHook.TransactionInfo.TransactionCode,
                    Id = Path.GetFileNameWithoutExtension(file),
                    ReceivedTime = fileInfo.CreationTime,
                    ReceivedDateStr = GetDateString(fileInfo.CreationTime),
                    ReceivedTimeStr = GetTimeString(fileInfo.CreationTime),
                });
            }

            webHooks = webHooks.OrderByDescending(wh => wh.ReceivedTime).ToList();

            var result = new SearchWebHookResponseModel()
            {
                Success = true,
                Data = webHooks
            };
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetWebHook(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json(new WebHookNotificationResponseModel()
                {
                    Success = false,
                    ErrorMessage = "Không tìm thấy file dữ liệu"
                });
            string filePath = WebHooksBasePath + Path.DirectorySeparatorChar + id + ".json";
            string jsonContent = string.Empty;
            try
            {
                jsonContent = await System.IO.File.ReadAllTextAsync(filePath);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Không thể đọc tệp tin webhook {0}", filePath);
                return Json(new WebHookNotificationResponseModel() {
                    Success = false,
                    ErrorMessage = "Không thể đọc file dữ liệu"
                });
            }
            FileInfo fileInfo = new FileInfo(filePath);
            var model = JsonConvert.DeserializeObject<WebHookNotificationResponseModel>(jsonContent);
            model.RawContent = jsonContent;
            model.ReceivedDateStr = GetDateString(fileInfo.CreationTime);
            model.ReceivedTimeStr = GetTimeString(fileInfo.CreationTime);
            model.Success = true;
            model.ValidChecksum = AlePayAPIClient.VerifyWebhookNotify(model);
            return Json(model);
        }

        [HttpPost]
        public string AlePayPostBack([FromBody] WebHookNotification model)
        {
            //var logFileName = string.Format("logs{0}alepay_webhook_log_{1}-{2}-{3}-{4}-{5}-{6}-{7}.log",
            //    Path.DirectorySeparatorChar,
            //    DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour,
            //    DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            //using (var logFile = System.IO.File.OpenWrite(logFileName))
            //{
            //    Request.Body.CopyToAsync(logFile);
            //}

            if (model.TransactionInfo == null)
                return "NOK! TransactionInfo is null";

            if (string.IsNullOrEmpty(model.TransactionInfo.TransactionCode))
                return "NOK! TransactionCode is empty";

            string webHookLogFile = string.Empty;
            int webHookSuffix = 0;
            do
            {
                string suffix = webHookSuffix == 0 ? string.Empty : string.Format("-{0}", webHookSuffix);
                webHookLogFile = string.Format("webhooks{0}{1}{2}.json",
                    Path.DirectorySeparatorChar, model.TransactionInfo.TransactionCode, suffix);
                webHookSuffix++;
            }
            while (System.IO.File.Exists(webHookLogFile));
            using (var hookLog = System.IO.File.OpenWrite(webHookLogFile))
            {
                var jsonSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented
                };
                string modelJson = JsonConvert.SerializeObject(model, jsonSettings);
                using (var writer = new StreamWriter(hookLog))
                {
                    writer.WriteAsync(modelJson);
                }
            }
            return "OK";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static string GetDateString(DateTime? datetime)
        {
            if (!datetime.HasValue)
                return string.Empty;
            return datetime.Value.ToString("dd/MM/yyyy");
        }

        private static string GetTimeString(DateTime? datetime)
        {
            if (!datetime.HasValue)
                return string.Empty;
            return datetime.Value.ToString("HH:mm:ss");
        }
    }
}
