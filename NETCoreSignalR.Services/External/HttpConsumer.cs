using LanguageExt;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETCoreSignalR.Services.External
{
    public interface IHttpConsumer
    {
        void AddCookies(List<Cookie> cookies);
        void AddHeaders(KeyValuePair<string, string> header);
        void AddHeaders(List<KeyValuePair<string, string>> headers);
        Option<T> Delete<T>(string url);
        Option<T> Delete<T>(string url, List<KeyValuePair<string, object>> param = null);
        Task<Option<T>> DeleteAsync<T>(string url, CancellationToken cancellationToken = default);
        Task<Option<T>> DeleteAsync<T>(string url, CancellationToken cancellationToken = default, List<KeyValuePair<string, object>> param = null);
        Option<T> Get<T>(string url);
        Option<T> Get<T>(string url, List<KeyValuePair<string, object>> param = null);
        Task<Option<T>> GetAsync<T>(string url, CancellationToken cancellationToken = default);
        Task<Option<T>> GetAsync<T>(string url, List<KeyValuePair<string, object>> param = null, CancellationToken cancellationToken = default);
        Option<T> Post<T>(string url, List<KeyValuePair<string, object>> param);
        Option<T> Post<T>(string url, object param);
        Task<Option<T>> PostAsync<T>(string url, List<KeyValuePair<string, object>> param, CancellationToken cancellationToken = default);
        Task<Option<T>> PostAsync<T>(string url, object param, CancellationToken cancellationToken = default);
        Option<T> Put<T>(string url, List<KeyValuePair<string, object>> param);
        Option<T> Put<T>(string url, object param);
        Task<Option<T>> PutAsync<T>(string url, List<KeyValuePair<string, object>> param, CancellationToken cancellationToken = default);
        Task<Option<T>> PutAsync<T>(string url, object param, CancellationToken cancellationToken = default);
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
        public Option<T> Get<T>(string url)
        {
            var request = new RestRequest(url, Method.GET, _defaultDataFormat);

            var response = _request.Get<T>(request);

            return response.IsSuccessful ? response.Data : Option<T>.None;
        }

        public Option<T> Post<T>(string url, object param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);
            request.AddJsonBody(param);

            var response = _request.Post<T>(request);

            return response.IsSuccessful ? response.Data : Option<T>.None;
        }

        public Option<T> Post<T>(string url, List<KeyValuePair<string, object>> param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _request.Post<T>(request);

            return response.IsSuccessful ? response.Data : Option<T>.None;
        }

        public Option<T> Put<T>(string url, object param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);
            request.AddJsonBody(param);

            var response = _request.Put<T>(request);

            return response.IsSuccessful ? response.Data : Option<T>.None;
        }

        public Option<T> Put<T>(string url, List<KeyValuePair<string, object>> param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _request.Put<T>(request);

            return response.IsSuccessful ? response.Data : Option<T>.None;
        }
        public Option<T> Get<T>(string url, List<KeyValuePair<string, object>> param = null)
        {
            var request = new RestRequest(url, Method.GET, _defaultDataFormat);

            if (param != null)
            {
                param.ForEach(p => request.AddParameter(p.Key, p.Value, ParameterType.GetOrPost));
            }

            var response = _request.Get<T>(request);

            return response.IsSuccessful ? response.Data : Option<T>.None;
        }


        public async Task<Option<T>> GetAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.GET, _defaultDataFormat);

            var response = await _request.ExecuteGetAsync<T>(request, cancellationToken);

            return response.IsSuccessful ? response.Data : Option<T>.None;
        }
        public async Task<Option<T>> PostAsync<T>(string url, object param, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.POST, _defaultDataFormat);
            request.AddJsonBody(param);

            var response = await _request.ExecutePostAsync<T>(request, cancellationToken);

            return response.IsSuccessful ? response.Data : Option<T>.None;
        }
        public async Task<Option<T>> PostAsync<T>(string url, List<KeyValuePair<string, object>> param, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _request.ExecutePostAsync<T>(request, cancellationToken);

            return response.IsSuccessful ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> GetAsync<T>(string url, List<KeyValuePair<string, object>> param = null, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.GET, _defaultDataFormat);
            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _request.ExecuteGetAsync<T>(request, cancellationToken);

            return response.IsSuccessful ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> PutAsync<T>(string url, List<KeyValuePair<string, object>> param, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _request.PutAsync<T>(request, cancellationToken);

            return response ?? Option<T>.None;
        }

        public async Task<Option<T>> PutAsync<T>(string url, object param, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            request.AddJsonBody(param);

            var response = await _request.PutAsync<T>(request, cancellationToken);

            return response ?? Option<T>.None;
        }

        public Option<T> Delete<T>(string url)
        {
            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);
            var response = _request.Delete<T>(request);

            return response.IsSuccessful ? response.Data : Option<T>.None;
        }

        public Option<T> Delete<T>(string url, List<KeyValuePair<string, object>> param = null)
        {
            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _request.Delete<T>(request);

            return response.IsSuccessful ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> DeleteAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);
            var response = await _request.DeleteAsync<T>(request);

            return response ?? Option<T>.None;
        }

        public async Task<Option<T>> DeleteAsync<T>(string url, CancellationToken cancellationToken = default, List<KeyValuePair<string, object>> param = null)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _request.DeleteAsync<T>(request);

            return response ?? Option<T>.None;
        }
        public void AddCookies(List<Cookie> cookies)
        {
            _request.CookieContainer = new CookieContainer();

            cookies.ForEach(cookie => _request.CookieContainer.Add(cookie));
        }

        public void AddHeaders(List<KeyValuePair<string, string>> headers)
        {
            headers.ForEach(header => _request.AddDefaultHeader(header.Key, header.Value));
        }
        public void AddHeaders(KeyValuePair<string, string> header)
        {
            _request.AddDefaultHeader(header.Key, header.Value);
        }

    }
}
