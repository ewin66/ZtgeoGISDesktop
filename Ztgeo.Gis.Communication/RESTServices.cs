﻿using Abp.Dependency;
using Castle.Core.Logging;
using Castle.MicroKernel.Util;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Ztgeo.Gis.Communication.Configuration;

namespace Ztgeo.Gis.Communication
{
    public class RESTServices : IRESTServices  
    { 
        public ILogger Logger { get; set; }

        private HttpInterceptConfiguration _httpInterceptConfiguration;
        public RESTServices(HttpInterceptConfiguration httpInterceptConfiguration)
        {
            _httpInterceptConfiguration = httpInterceptConfiguration;
            Logger = NullLogger.Instance;
        }
        public string Get(Uri url, int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null)
        {
            string responseContent= Request(Method.GET, url, timeout, null, null, additionalHeaders);
            if (this._httpInterceptConfiguration.OnAfterRequest != null)
            {
                return this._httpInterceptConfiguration.OnAfterRequest(responseContent);
            }
            else
            {
                return responseContent;
            }
        }

        public OutModel Get<OutModel>(Uri url, int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null) { 
            string responseContent = Request(Method.GET, url, timeout, null, null, additionalHeaders);
            if (this._httpInterceptConfiguration.OnAfterRequest != null)
            {
                return JsonConvert.DeserializeObject<OutModel>(this._httpInterceptConfiguration.OnAfterRequest(responseContent));
            }
            else
            {
                return JsonConvert.DeserializeObject<OutModel>(responseContent);
            } 
        }

        public string Post(Uri uri, string requestContent, int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null)
        {
            string responseContent = Request(Method.POST, uri, timeout, requestContent, null, additionalHeaders);
            if (this._httpInterceptConfiguration.OnAfterRequest != null)
            {
                return this._httpInterceptConfiguration.OnAfterRequest(responseContent);
            }
            else
            {
                return responseContent;
            }
        }

        public OutModel Post<OutModel>(Uri uri, object inputModel, int timeout = 30, IEnumerable<KeyValuePair<string, string>> additionalHeaders = null) {
            string responseContent = Request(Method.POST, uri, timeout, JsonConvert.SerializeObject(inputModel), null, additionalHeaders);
            if (this._httpInterceptConfiguration.OnAfterRequest != null)
            {
                return JsonConvert.DeserializeObject<OutModel>(this._httpInterceptConfiguration.OnAfterRequest(responseContent));
            }
            else
            {
                return JsonConvert.DeserializeObject<OutModel>(responseContent);
            } 
        }

        private string Request(Method method, Uri uri, int timeout, string requestContent = null, string contentType = null,
        IEnumerable<KeyValuePair<string, string>> additionalHeaders = null)
        {
            var client = new RestClient(uri);
            client.Timeout =  timeout ;
            var request = new RestRequest(Method.POST);
            if (this._httpInterceptConfiguration.OnBeforeRequest != null)
            {
                request.OnBeforeRequest = this._httpInterceptConfiguration.OnBeforeRequest;
            }
            request.AddHeader("Content-Type", contentType ?? "application/json");
            Logger.Info(string.Format("Http Request: \r\n{0}", requestContent));  
            if (!string.IsNullOrEmpty(contentType))
                request.AddParameter(contentType ?? "application/json", requestContent, ParameterType.RequestBody);
            if (additionalHeaders != null)
            {
                Logger.Info(string.Format("Http Request Header: \r\n{0}", JsonConvert.SerializeObject(additionalHeaders)));
                foreach (KeyValuePair<string,string> additionHeader in additionalHeaders)
                {
                    request.AddHeader(additionHeader.Key, additionHeader.Value);
                }
            }
            IRestResponse response = client.Execute(request);
            if (!string.IsNullOrEmpty(response.ErrorMessage) || response.ErrorException != null)
            { 
                Logger.Error(response.ErrorMessage, response.ErrorException);
                throw response.ErrorException;
            }
            Logger.Info(string.Format("Http Response: \r\n{0}", response.Content));
            return response.Content; 
        }
    }
}
