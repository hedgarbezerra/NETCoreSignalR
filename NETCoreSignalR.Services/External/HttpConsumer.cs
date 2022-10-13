using Dawn;
using LanguageExt;
using NETCoreSignalR.Domain.Interfaces;
using Polly;
using Polly.Contrib.WaitAndRetry;
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
  
    public class HttpConsumer : IHttpConsumer
    {
        private readonly IRestClient _requestClient;
        private DataFormat _defaultDataFormat;
        private readonly IAsyncPolicy<IRestResponse> _policy;

        public HttpConsumer(IRestClient client)
        {
            Guard.Argument<IRestClient>(client)
                   .NotNull();

            _requestClient = client;
            _defaultDataFormat = DataFormat.Json;
            _policy = Policy<IRestResponse>.Handle<Exception>()
            .OrResult(r => !r.IsSuccessful || r.StatusCode >= HttpStatusCode.InternalServerError)
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 3));
        }
        public HttpConsumer(IRestClient client, DataFormat dataFormat)
        {
            Guard.Argument<IRestClient>(client)
                .NotNull();

            _requestClient = client;
            _defaultDataFormat = dataFormat;
        }

        public Option<T> Get<T>(string url) 
        {
            var polly = Policy<IRestResponse<T>>.Handle<Exception>()
            .OrResult(r => !r.IsSuccessful || r.StatusCode >= HttpStatusCode.InternalServerError)
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 3));
            var request = new RestRequest(url, Method.GET, _defaultDataFormat);
            var result = polly.ExecuteAsync(() => _requestClient.ExecuteAsync<T>(request, Method.GET));
            var response = _requestClient.Execute<T>(request, Method.GET);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }
        public Option<T> Get<T>(string url, List<KeyValuePair<string, object>> param = null)
        {
            var request = new RestRequest(url, Method.GET, _defaultDataFormat);

            if (param != null)
            {
                param.ForEach(p => request.AddParameter(p.Key, p.Value, ParameterType.GetOrPost));
            }

            var response = _requestClient.Execute<T>(request, Method.GET);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public Option<T> Post<T>(string url, object param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);
            request.AddJsonBody(param);

            var response = _requestClient.Execute<T>(request, Method.POST);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public Option<T> Post<T>(string url, List<KeyValuePair<string, object>> param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _requestClient.Post<T>(request);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public Option<T> Put<T>(string url, object param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);
            request.AddJsonBody(param);

            var response = _requestClient.Execute<T>(request, Method.PUT);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public Option<T> Put<T>(string url, List<KeyValuePair<string, object>> param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _requestClient.Execute<T>(request, Method.PUT);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }
        
        public Option<T> Delete<T>(string url)
        {
            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);
            var response = _requestClient.Execute<T>(request, Method.DELETE);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public Option<T> Delete<T>(string url, List<KeyValuePair<string, object>> param = null)
        {
            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _requestClient.Execute<T>(request, Method.DELETE);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> GetAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.GET, _defaultDataFormat);

            var response = await _requestClient.ExecuteAsync<T>(request, Method.GET, cancellationToken);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> GetAsync<T>(string url, List<KeyValuePair<string, object>> param = null, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.GET, _defaultDataFormat);
            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _requestClient.ExecuteAsync<T>(request, Method.GET, cancellationToken);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> PostAsync<T>(string url, object param, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.POST, _defaultDataFormat);
            request.AddJsonBody(param);

            var response = await _requestClient.ExecuteAsync<T>(request, Method.POST, cancellationToken);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }
        
        public async Task<Option<T>> PostAsync<T>(string url, List<KeyValuePair<string, object>> param, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _requestClient.ExecuteAsync<T>(request, Method.POST, cancellationToken);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> PutAsync<T>(string url, List<KeyValuePair<string, object>> param, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _requestClient.ExecuteAsync<T>(request, Method.PUT, cancellationToken);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> PutAsync<T>(string url, object param, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            request.AddJsonBody(param);

            var response = await _requestClient.ExecuteAsync<T>(request, Method.PUT, cancellationToken);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> DeleteAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);
            var response = await _requestClient.ExecuteAsync<T>(request, Method.DELETE);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> DeleteAsync<T>(string url, CancellationToken cancellationToken = default, List<KeyValuePair<string, object>> param = null)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _requestClient.ExecuteAsync<T>(request, Method.DELETE);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public void AddCookie(List<Cookie> cookies)
        {
            _requestClient.CookieContainer ??= new CookieContainer();
            cookies.ForEach(cookie => _requestClient.CookieContainer.Add(cookie));
        }

        public void AddHeader(List<KeyValuePair<string, string>> headers)  =>
            headers.ForEach(header => _requestClient.AddDefaultHeader(header.Key, header.Value));
        public void AddHeader(KeyValuePair<string, string> header) =>
            _requestClient.AddDefaultHeader(header.Key, header.Value);
    }
}
