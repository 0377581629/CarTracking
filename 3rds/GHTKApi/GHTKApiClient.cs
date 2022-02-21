using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GHTK.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;

namespace GHTK
{
    public class GHTKApiClient
    {
        #region Constructor

        private RestClient _restClient;
        private readonly string _url = @"https://services.giaohangtietkiem.vn";
        private readonly string _token = "a5F81c906c42F4a2c052bFB03Cf8d5120F9eDd99";

        public GHTKApiClient(string baseUrl = null, string token = null)
        {
            if (!string.IsNullOrEmpty(baseUrl))
                _url = baseUrl;
            if (!string.IsNullOrEmpty(token))
                _token = token;
        }

        #endregion

        // Get Address 3 level
        public async Task<List<Province>> GetAddresses()
            => await HandleCustomApi<List<Province>>("https://provinces.open-api.vn",
                "/api/", Method.GET, parameters: new Dictionary<string, string>
                {
                    { "depth", "3" }
                });

        // Get Level 4 Addresses
        public async Task<List<string>> GetAddressesLevel4(string provinceName, string districtName, string wardName)
            => await HandleCommonApi<List<string>>(
                "/services/address/getAddressLevel4", Method.GET, parameters: new Dictionary<string, string>
                {
                    { "province", provinceName },
                    { "district", districtName },
                    { "ward_street", wardName }
                });

        // Get Pick Address
        public async Task<List<PickAddress>> GetPickAddresses()
            => await HandleCommonApi<List<PickAddress>>("/services/shipment/list_pick_add", Method.GET);

        // Order
        public async Task<OrderResponseModel> CreateOrder(Order input)
            => await HandleOrderApi<OrderResponseModel>("/services/shipment/order/?ver=1.5", Method.POST, new CreateOrderRequestModel { Order = input });

        public async Task<OrderStatus> GetOrderStatus(string ghtkLabelId)
            => await HandleOrderApi<OrderStatus>($"/services/shipment/v2/{ghtkLabelId}", Method.GET);

        public async Task CancelOrder(string ghtkLabelId) 
            => await HandleOrderApi($"/services/shipment/v2/{ghtkLabelId}", Method.POST);
        
        public async Task<PrintOrder> PrintLabel(string ghtkLabelId)
            => await HandlePrintOrder($"/services/label/{ghtkLabelId}", Method.GET);
        
        #region Private Methods

        private RestClient GetRestClient()
        {
            return _restClient ?? (_restClient = new RestClient(_url));
        }

        private async Task<T> HandleCommonApi<T>(string requestPath, Method requestType, RequestModel model = null, Dictionary<string, string> parameters = null)
        {
            var request = new RestRequest("/" + requestPath, requestType)
            {
                JsonSerializer = new RestSharpJsonNetSerializer()
            };

            request.AddHeader("Token", _token);

            if (model != null)
                request.AddJsonBody(model);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }

            var fullUrl = GetRestClient().BuildUri(request);
            Console.WriteLine(fullUrl);

            var response = await GetRestClient().ExecuteAsync(request);

            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
                return (T)Activator.CreateInstance(typeof(T));

            var resModel = JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content);
            if (resModel != null)
                return resModel.Data;

            return (T)Activator.CreateInstance(typeof(T));
        }

        private async Task HandleOrderApi(string requestPath, Method requestType, RequestModel model = null, Dictionary<string, string> parameters = null)
        {
            var request = new RestRequest("/" + requestPath, requestType)
            {
                JsonSerializer = new RestSharpJsonNetSerializer()
            };

            request.AddHeader("Token", _token);

            if (model != null)
                request.AddJsonBody(model);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }

            var fullUrl = GetRestClient().BuildUri(request);
            Console.WriteLine(fullUrl);

            var response = await GetRestClient().ExecuteAsync(request);

            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content)) throw new Exception("Error");
            var resModel = JsonConvert.DeserializeObject<SimpleResponseModel>(response.Content);
            if (resModel != null && !resModel.Success)
                throw new Exception(resModel.Message);
            throw new Exception("Error");
        }

        private async Task<T> HandleOrderApi<T>(string requestPath, Method requestType, RequestModel model = null, Dictionary<string, string> parameters = null)
        {
            var request = new RestRequest("/" + requestPath, requestType)
            {
                JsonSerializer = new RestSharpJsonNetSerializer()
            };

            request.AddHeader("Token", _token);

            if (model != null)
                request.AddJsonBody(model);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }

            var fullUrl = GetRestClient().BuildUri(request);
            Console.WriteLine(fullUrl);

            var response = await GetRestClient().ExecuteAsync(request);

            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
                return (T)Activator.CreateInstance(typeof(T));

            var resModel = JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content);
            if (resModel != null)
                return resModel.Order;

            return (T)Activator.CreateInstance(typeof(T));
        }

        private async Task<T> HandleCustomApi<T>(string baseUrl, string requestPath, Method requestType, RequestModel model = null, Dictionary<string, string> parameters = null)
        {
            var request = new RestRequest("/" + requestPath, requestType)
            {
                JsonSerializer = new RestSharpJsonNetSerializer()
            };

            request.AddHeader("Token", _token);

            if (model != null)
                request.AddJsonBody(model);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }

            var client = new RestClient(baseUrl);
            var fullUrl = client.BuildUri(request);
            Console.WriteLine(fullUrl);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
                return (T)Activator.CreateInstance(typeof(T));

            var resModel = JsonConvert.DeserializeObject<T>(response.Content);
            if (resModel != null)
                return resModel;

            return (T)Activator.CreateInstance(typeof(T));
        }
        
        private async Task<PrintOrder> HandlePrintOrder(string requestPath, Method requestType, RequestModel model = null, Dictionary<string, string> parameters = null)
        {
            var request = new RestRequest("/" + requestPath, requestType)
            {
                JsonSerializer = new RestSharpJsonNetSerializer()
            };

            request.AddHeader("Token", _token);

            if (model != null)
                request.AddJsonBody(model);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }

            var fullUrl = GetRestClient().BuildUri(request);
            Console.WriteLine(fullUrl);

            var res = new PrintOrder();
            
            var response = await GetRestClient().ExecuteAsync(request);

            if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
                return res;
            if (response.ContentType != "application/pdf")
            {
                var simpleResponse = JsonConvert.DeserializeObject<SimpleResponseModel>(response.Content);
                if (simpleResponse != null)
                {
                    res.Success = simpleResponse.Success;
                    res.Message = simpleResponse.Message;
                }
                return res;
            }

            res.Success = true;
            res.LabelPdfFileBlob = response.RawBytes;
            return res;
        }

        /// 
        /// Default JSON serializer for request bodies
        /// Doesn't currently use the SerializeAs attribute, defers to Newtonsoft's attributes
        /// 
        private class RestSharpJsonNetSerializer : ISerializer
        {
            private readonly JsonSerializer _serializer;

            /// 
            /// Default serializer
            /// 
            public RestSharpJsonNetSerializer()
            {
                ContentType = "application/json";
                _serializer = new JsonSerializer
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Include,
                    DefaultValueHandling = DefaultValueHandling.Include
                };
            }

            /// 
            /// Default serializer with overload for allowing custom Json.NET settings
            /// 
            public RestSharpJsonNetSerializer(JsonSerializer serializer)
            {
                ContentType = "application/json";
                _serializer = serializer;
            }

            /// 
            /// Serialize the object as JSON
            /// 
            /// Object to serialize
            /// JSON as String
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

            /// 
            /// Unused for JSON Serialization
            /// 

            public string DateFormat { get; set; }

            /// 
            /// Unused for JSON Serialization
            /// 

            public string RootElement { get; set; }

            /// 
            /// Unused for JSON Serialization
            /// 

            public string Namespace { get; set; }

            /// 
            /// Content type for serialized content
            /// 

            public string ContentType { get; set; }
        }

        #endregion
    }
}