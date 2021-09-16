using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NETCoreSignalR.Services.External
{
    public interface IHttpConsumer
    {
        void AddCookies(List<Cookie> cookies);
        void AddHeaders(KeyValuePair<string, string> header);
        void AddHeaders(List<KeyValuePair<string, string>> headers);
        T Get<T>(string url);
        T Get<T>(string url, List<KeyValuePair<string, object>> param = null);
        Task<T> GetAsync<T>(string url);
        Task<T> GetAsync<T>(string url, List<KeyValuePair<string, object>> param = null);
        T Post<T>(string url, List<KeyValuePair<string, object>> param);
        T Post<T>(string url, object param);
        Task<T> PostAsync<T>(string url, List<KeyValuePair<string, object>> param);
        Task<T> PostAsync<T>(string url, object param);
        T Put<T>(string url, List<KeyValuePair<string, object>> param);
        T Put<T>(string url, object param);
        Task<T> PutAsync<T>(string url, List<KeyValuePair<string, object>> param);
        Task<T> PutAsync<T>(string url, object param);
        T Delete<T>(string url);
        T Delete<T>(string url, List<KeyValuePair<string, object>> param = null);
        Task<T> DeleteAsync<T>(string url);
        Task<T> DeleteAsync<T>(string url, List<KeyValuePair<string, object>> param = null);
    }

    //Excluded from code coverage due to inability to test the extension method 
    [ExcludeFromCodeCoverage]
    public class HttpConsumer : IHttpConsumer
    {
        private readonly IRestClient _request;
        private DataFormat _defaultDataFormat;
        public HttpConsumer(IRestClient client)
        {
            _request = client;
            _defaultDataFormat = DataFormat.Json;
        }
        public HttpConsumer(IRestClient client, DataFormat dataFormat)
        {
            _request = client;
            _defaultDataFormat = dataFormat;
        }
        public T Get<T>(string url)
        {
            var request = new RestRequest(url, Method.GET, _defaultDataFormat);

            var response = _request.Get<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Something went wrong while connecting to {response.ResponseUri}");
        }

        public T Post<T>(string url, object param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);
            request.AddJsonBody(param);

            var response = _request.Post<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Something went wrong while connecting to {response.ResponseUri}");
        }

        public T Post<T>(string url, List<KeyValuePair<string, object>> param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _request.Post<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Something went wrong while connecting to {response.ResponseUri}");
        }

        public T Put<T>(string url, object param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);
            request.AddJsonBody(param);

            var response = _request.Put<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Something went wrong while connecting to {response.ResponseUri}");
        }

        public T Put<T>(string url, List<KeyValuePair<string, object>> param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _request.Put<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Something went wrong while connecting to {response.ResponseUri}");
        }
        public T Get<T>(string url, List<KeyValuePair<string, object>> param = null)
        {
            var request = new RestRequest(url, Method.GET, _defaultDataFormat);

            if (param != null)
            {
                param.ForEach(p => request.AddParameter(p.Key, p.Value, ParameterType.GetOrPost));
            }

            var response = _request.Get<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Something went wrong while connecting to {response.ResponseUri}");
        }


        public async Task<T> GetAsync<T>(string url)
        {
            var request = new RestRequest(url, Method.GET, _defaultDataFormat);

            var response = await _request.ExecuteGetAsync<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Something went wrong while connecting to {response.ResponseUri}");
        }
        public async Task<T> PostAsync<T>(string url, object param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);
            request.AddJsonBody(param);

            var response = await _request.ExecutePostAsync<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Something went wrong while connecting to {response.ResponseUri}");
        }
        public async Task<T> PostAsync<T>(string url, List<KeyValuePair<string, object>> param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _request.ExecutePostAsync<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Something went wrong while connecting to {response.ResponseUri}");
        }

        public Task<T> GetAsync<T>(string url, List<KeyValuePair<string, object>> param = null)
        {
            throw new NotImplementedException();
        }

        public async Task<T> PutAsync<T>(string url, List<KeyValuePair<string, object>> param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _request.PutAsync<T>(request);

            return response;
        }

        public async Task<T> PutAsync<T>(string url, object param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            request.AddJsonBody(param);

            var response = await _request.PutAsync<T>(request);

            return response;
        }

        public T Delete<T>(string url)
        {
            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);
            var response = _request.Delete<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Something went wrong while connecting to {response.ResponseUri}");
        }

        public T Delete<T>(string url, List<KeyValuePair<string, object>> param = null)
        {
            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _request.Delete<T>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new Exception($"Something went wrong while connecting to {response.ResponseUri}");
        }

        public Task<T> DeleteAsync<T>(string url)
        {
            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);
            var response = _request.DeleteAsync<T>(request);

            return response;
        }

        public Task<T> DeleteAsync<T>(string url, List<KeyValuePair<string, object>> param = null)
        {
            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _request.DeleteAsync<T>(request);

            return response;
        }
        public void AddCookies(List<Cookie> cookies)
        {
            this._request.CookieContainer = new CookieContainer();

            cookies.ForEach(cookie => this._request.CookieContainer.Add(cookie));
        }

        public void AddHeaders(List<KeyValuePair<string, string>> headers)
        {
            headers.ForEach(header => this._request.AddDefaultHeader(header.Key, header.Value));
        }
        public void AddHeaders(KeyValuePair<string, string> header)
        {
            this._request.AddDefaultHeader(header.Key, header.Value);
        }
        
    }
}
