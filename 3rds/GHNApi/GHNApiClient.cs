using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GHN.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace GHN
{
    public class GHNApiClient
    {
        #region Constructor

        private RestClient _restClient;
        private readonly string _url = @"https://online-gateway.ghn.vn/shiip/public-api/";
        private readonly string _token = "a592c6f4-7f8e-11ec-b18b-3a9c67615aba";
        private readonly string _shopId = "";

        public GHNApiClient(string baseUrl = null, string token = null, string shopId = null)
        {
            if (!string.IsNullOrEmpty(baseUrl))
                _url = baseUrl;
            if (!string.IsNullOrEmpty(token))
                _token = token;
            if (!string.IsNullOrEmpty(shopId))
                _shopId = shopId;
        }

        #endregion

        #region Address

        // Get Provinces
        public async Task<List<Province>> GetProvinces()
            => await HandleCommonApi<List<Province>>("master-data/province", Method.GET);

        // Get Districts
        public async Task<List<District>> GetDistricts(uint provinceId)
            => await HandleCommonApi<List<District>>("master-data/district", Method.GET, parameters: new Dictionary<string, string>
            {
                { "province_id", provinceId.ToString() }
            });

        //Get wards
        public async Task<List<Ward>> GetWards(uint districtId)
            => await HandleCommonApi<List<Ward>>("master-data/ward", Method.GET, parameters: new Dictionary<string, string>
            {
                { "district_id", districtId.ToString() }
            });

        #endregion

        #region Store

        // Get Stores
        public async Task<List<Store>> GetStores()
            => (await HandleCommonApi<StoreResponseModel>("v2/shop/all", Method.GET, parameters: new Dictionary<string, string>
            {
                { "offset", "0" },
                { "limit", "200" },
                { "client_phone", "" }
            })).Stores;

        // Create Store
        public async Task<ulong> CreateStore(CreateStoreRequestModel input)
            => (await HandleCommonApi<CreateStoreResponseModel>("v2/shop/register", Method.POST, input)).ShopId;

        #endregion

        #region Order

        public async Task<SearchOrderResponseModel> GetOrders(SearchOrderRequestModel input)
            => await HandleCommonApi<SearchOrderResponseModel>("v2/shipping-order/search", Method.POST, input);

        #endregion

        #region Station
        public async Task<List<Station>> GetStations(GetStationRequestModel input)
            => await HandleCommonApi<List<Station>>("v2/station/get", Method.POST, input);

        #endregion
        
        #region Pick shift

        public async Task<List<PickShift>> GetPickShifts()
            => await HandleCommonApi<List<PickShift>>("v2/shift/date", Method.GET);

        #endregion

        #region Services

        public async Task<List<Service>> GetServices(uint shopId, uint fromDistrictId, uint toDistrictId)
            => await HandleCommonApi<List<Service>>("v2/shipping-order/available-services", Method.GET, parameters: new Dictionary<string, string>
            {
                { "shop_id", shopId.ToString() },
                { "from_district", fromDistrictId.ToString() },
                { "to_district", toDistrictId.ToString() }
            });

        #endregion
        
        #region Fee
        public async Task<FeeCalculationResponseModel> FeeCalculation(FeeCalculationRequestModel input)
            => await HandleCommonApi<FeeCalculationResponseModel>("v2/shipping-order/fee", Method.POST, input);
        #endregion

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

            request.AddHeader("token", _token);
            if (!string.IsNullOrEmpty(_shopId))
                request.AddHeader("ShopId", _shopId);
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

        /// 
        /// Default JSON serializer for request bodies
        /// Doesn't currently use the SerializeAs attribute, defers to Newtonsoft's attributes
        /// 
        private class RestSharpJsonNetSerializer : ISerializer
        {
            private readonly Newtonsoft.Json.JsonSerializer _serializer;

            /// 
            /// Default serializer
            /// 
            public RestSharpJsonNetSerializer()
            {
                ContentType = "application/json";
                _serializer = new Newtonsoft.Json.JsonSerializer
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Include,
                    DefaultValueHandling = DefaultValueHandling.Include
                };
            }

            /// 
            /// Default serializer with overload for allowing custom Json.NET settings
            /// 
            public RestSharpJsonNetSerializer(Newtonsoft.Json.JsonSerializer serializer)
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