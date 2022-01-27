using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using RestSharp.Serializers;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using alepay.Attributes;
using alepay.Models;

namespace alepay
{
    /// <summary>
    /// APIClient giao tiếp với cổng thanh toán AlePay
    /// Được viết dựa theo: TÀI LIỆU TÍCH HỢP THANH TOÁN NGAY + TRẢ GÓP ALEPAY version 3.0
    /// </summary>
    public class AlePayApiClient
    {
        #region Constructor

        private RestClient _restClient;
        
        private string BaseUrl { get; set; }
        
        private string TokenKey { get; set; }
        
        private string ChecksumKey { get; set; }

        
        public AlePayApiClient(string baseUrl, string tokenKey, string checksumKey)
        {
            BaseUrl = baseUrl;
            TokenKey = tokenKey;
            ChecksumKey = checksumKey;
        }
        
        #endregion

        /// <summary>
        /// Khởi tạo thanh toán
        /// </summary>
        /// <returns></returns>
        public async Task<RequestPaymentResponse> RequestPaymentAsync(RequestPaymentRequestModel model)
            => await HandleCommonAPI<RequestPaymentResponse>("request-payment", Method.POST, model);

        /// <summary>
        /// Truy vấn trạng thái giao dịch
        /// </summary>
        public async Task<GetTransactionInfoResponseModel>
            GetTransactionInfoAsync(GetTransactionInfoRequestModel model)
                => await HandleCommonAPI<GetTransactionInfoResponseModel>(
                    "get-transaction-info", Method.POST, model);

        /// <summary>
        /// Lấy thông tin kỳ hạn, phí trả góp
        /// </summary>
        public async Task<GetInstallmentResponseModel> GetInstallmentInfoAsync(GetInstallmentRequestModel model)
            => await HandleCommonAPI<GetInstallmentResponseModel>("get-installment-info", Method.POST, model);

        /// <summary>
        /// Lấy danh sách ngân hàng
        /// </summary>
        public async Task<GetListBanksResponseModel> GetListBanksAsync()
            => await HandleCommonAPI<GetListBanksResponseModel>(
                "get-list-banks", Method.POST, new RequestModel());

        /// <summary>
        /// Xử lý thông tin WebHook
        /// </summary>
        public WebHookNotification ParseWebHookAsync(string requestBody)
            => JsonConvert.DeserializeObject<WebHookNotification>(requestBody);

        /// <summary>
        /// Hàm tiện ích lấy ra nội dung lỗi từ mã lỗi
        /// </summary>
        /// <param name="code">Mã lỗi</param>
        public static string GetErrorMessageForCode(int code)
        {
            var message = string.Empty;
            if (!AlePayDefs.ErrorCodes.TryGetValue(code, out message))
                message = "Không xác định";
            return message;
        }

        // TODO(trieupt): Cần kiểm tra vấn đề timezone
        /// <summary>
        /// Hàm tiện ích để chuyển đổi thời gian kiểu unix sang kiểu DateTime của Windows
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime? UnixTimeStampMilisecsToDateTime(long? unixTimeStamp)
        {
            if (!unixTimeStamp.HasValue)
                return null;
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp.Value).ToLocalTime();
            return dtDateTime;
        }

        public bool VerifyWebhookNotify(WebHookNotification notify)
        {
            if (notify == null)
                return false;
            if (string.IsNullOrEmpty(notify.Checksum))
                return false;
            if (notify.TransactionInfo == null)
                return false;
            var checkSumStr = notify.TransactionInfo.OrderCode + notify.TransactionInfo.Amount + 
                              notify.TransactionInfo.TransactionCode + ChecksumKey;
            var ourHash = MD5Hash(checkSumStr);
            return ourHash.Equals(notify.Checksum);
        }

        #region Private, helper methods, classes
        private RestClient GetRestClient()
        {
            return _restClient ?? (_restClient = new RestClient(BaseUrl));
        }

        private async Task<T> HandleCommonAPI<T>(string requestPath,
            Method requestType, RequestModel model)
        {
            model.TokenKey = TokenKey;
            model.Signature = CalculateSignature(model);
            var request = new RestRequest("/" + requestPath, requestType)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new RestSharpNewtonsoftJsonSerializer()
            };
            request.AddJsonBody(model);
            var response = await GetRestClient().ExecuteAsync<T>(request);
            return response.Data;
        }

        /// <summary>
        /// Calculate hmac
        /// </summary>
        /// <param name="modelObject">Object to calculate</param>
        /// <returns></returns>
        private string CalculateSignature(object modelObject)
        {
            var parameters = GetAPIParameters(modelObject);

            // Build queryString
            parameters = parameters.OrderBy(p => p.Key).ToList();
            var unsignedString = string.Empty;
            for (var i = 0; i < parameters.Count; i++)
            {
                if (i > 0)
                    unsignedString += '&';
                var parameter = parameters[i];
                unsignedString += string.Format("{0}={1}", parameter.Key, parameter.Value);
            }

            // Calculate hmac
            var hmac = new HMACSHA256(Encoding.Default.GetBytes(ChecksumKey));
            var resultBytes = hmac.ComputeHash(Encoding.Default.GetBytes(unsignedString));
            var result = string.Join("", resultBytes.ToList().Select(b => b.ToString("x2")).ToArray());
            return result;
        }

        private static string GetCamelCaseName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            var newName = Char.ToLowerInvariant(name[0]).ToString();
            if (name.Length > 1)
                newName += name.Substring(1);
            return newName;
        }

        private List<KeyValuePair<string, string>> GetAPIParameters(object modelObject)
        {
            // Get all parameters from model object
            var parameters = new List<KeyValuePair<string, string>>();

            var type = modelObject.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var paramInfo = GetAPIParamAttribute(property);
                if (paramInfo == null)
                    continue;

                // Get parameter name, if custom name not specified,
                // the parameter name is the property name
                var name = paramInfo.Name;
                if (string.IsNullOrEmpty(name))
                    name = property.Name;
                name = GetCamelCaseName(name);

                // Get property value, but the type of value is unknow.
                var value = property.GetValue(modelObject);
                var valueString = string.Empty;
                if (value is bool)
                {
                    valueString += ("" + value).ToLower();
                }
                else
                {
                    valueString += value;
                }
                parameters.Add(new KeyValuePair<string, string>(name, valueString));
            }
            return parameters.OrderBy(p => p.Key).ToList();
        }

        private static APIParamAttribute GetAPIParamAttribute(
            PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                return null;
            var attribute = propertyInfo.GetCustomAttribute<
                APIParamAttribute>(true);
            return attribute;
        }

        private bool VerifyResponseSignature(ResponseModel model) => true;

        /// <summary>
        /// Restsharp serialier using Newtonsoft.Json to serialize object with
        /// 'camelcase' property name rule
        /// </summary>
        public class RestSharpNewtonsoftJsonSerializer : ISerializer
        {
            private readonly Newtonsoft.Json.JsonSerializer _serializer;

            public RestSharpNewtonsoftJsonSerializer()
            {
                ContentType = "application/json";

                var contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };

                _serializer = new Newtonsoft.Json.JsonSerializer
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Include,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    ContractResolver = contractResolver
                };
            }

            public RestSharpNewtonsoftJsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
            {
                ContentType = "application/json";
                _serializer = serializer;
            }

            public string Serialize(object obj)
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                    {
                        jsonTextWriter.Formatting = Formatting.Indented;
                        jsonTextWriter.QuoteChar = '"';

                        _serializer.Serialize(jsonTextWriter, obj);

                        var result = stringWriter.ToString();
                        return result;
                    }
                }
            }

            public string ContentType { get; set; }
        }

        private static string MD5Hash(string input)
        {
            var hash = new StringBuilder();
            var md5provider = new MD5CryptoServiceProvider();
            var bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            foreach (var t in bytes)
            {
                hash.Append(t.ToString("x2"));
            }
            return hash.ToString();
        }
        #endregion // Private, helper methods, classes
    }
}
