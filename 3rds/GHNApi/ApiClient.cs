﻿using System;
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
    public class ApiClient
    {
        #region Constructor

        private RestClient _restClient;
        private readonly string _url = @"https://online-gateway.ghn.vn/shiip/public-api/";
        private readonly string _token = "e5347cba-7edc-11ec-b18b-3a9c67615aba";
        private readonly string _shopId = "";

        public ApiClient(string baseUrl = null, string token = null, string shopId = null)
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
        public async Task<List<District>> GetDistricts(int provinceId)
            => await HandleCommonApi<List<District>>("master-data/district", Method.GET, parameters: new Dictionary<string, string>
            {
                {"province_id",provinceId.ToString()}
            });

        //Get wards
        public async Task<List<Ward>> GetWards(int districtId)
            => await HandleCommonApi<List<Ward>>("master-data/ward", Method.GET, parameters: new Dictionary<string, string>
            {
                {"district_id",districtId.ToString()}
            });

        #endregion

        #region Private Methods

        private RestClient GetRestClient()
        {
            return _restClient ?? (_restClient = new RestClient(_url));
        }

        private async Task<T> HandleCommonApi<T>(string requestPath, Method requestType, RequestModel model = null, Dictionary<string,string> parameters = null)
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