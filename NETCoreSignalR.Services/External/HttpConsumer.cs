using Dawn;
using LanguageExt;
using NETCoreSignalR.Domain.Interfaces;
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
        private readonly IRestClient _request;
        private DataFormat _defaultDataFormat;
        public HttpConsumer(IRestClient client)
        {
            Guard.Argument<IRestClient>(client)
                   .NotNull();

            _request = client;
            _defaultDataFormat = DataFormat.Json;
        }
        public HttpConsumer(IRestClient client, DataFormat dataFormat)
        {
            Guard.Argument<IRestClient>(client)
                .NotNull();

            _request = client;
            _defaultDataFormat = dataFormat;
        }

        public Option<T> Get<T>(string url)
        {
            var request = new RestRequest(url, Method.GET, _defaultDataFormat);

            var response = _request.Execute<T>(request, Method.GET);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }
        public Option<T> Get<T>(string url, List<KeyValuePair<string, object>> param = null)
        {
            var request = new RestRequest(url, Method.GET, _defaultDataFormat);

            if (param != null)
            {
                param.ForEach(p => request.AddParameter(p.Key, p.Value, ParameterType.GetOrPost));
            }

            var response = _request.Execute<T>(request, Method.GET);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public Option<T> Post<T>(string url, object param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);
            request.AddJsonBody(param);

            var response = _request.Execute<T>(request, Method.POST);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public Option<T> Post<T>(string url, List<KeyValuePair<string, object>> param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _request.Post<T>(request);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public Option<T> Put<T>(string url, object param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);
            request.AddJsonBody(param);

            var response = _request.Execute<T>(request, Method.PUT);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public Option<T> Put<T>(string url, List<KeyValuePair<string, object>> param)
        {
            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _request.Execute<T>(request, Method.PUT);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }
        
        public Option<T> Delete<T>(string url)
        {
            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);
            var response = _request.Execute<T>(request, Method.DELETE);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public Option<T> Delete<T>(string url, List<KeyValuePair<string, object>> param = null)
        {
            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = _request.Execute<T>(request, Method.DELETE);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> GetAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.GET, _defaultDataFormat);

            var response = await _request.ExecuteAsync<T>(request, Method.GET, cancellationToken);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> GetAsync<T>(string url, List<KeyValuePair<string, object>> param = null, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.GET, _defaultDataFormat);
            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _request.ExecuteAsync<T>(request, Method.GET, cancellationToken);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> PostAsync<T>(string url, object param, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.POST, _defaultDataFormat);
            request.AddJsonBody(param);

            var response = await _request.ExecuteAsync<T>(request, Method.POST, cancellationToken);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }
        
        public async Task<Option<T>> PostAsync<T>(string url, List<KeyValuePair<string, object>> param, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _request.ExecuteAsync<T>(request, Method.POST, cancellationToken);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> PutAsync<T>(string url, List<KeyValuePair<string, object>> param, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _request.ExecuteAsync<T>(request, Method.PUT, cancellationToken);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> PutAsync<T>(string url, object param, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.POST, _defaultDataFormat);

            request.AddJsonBody(param);

            var response = await _request.ExecuteAsync<T>(request, Method.PUT, cancellationToken);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> DeleteAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);
            var response = await _request.ExecuteAsync<T>(request, Method.DELETE);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }

        public async Task<Option<T>> DeleteAsync<T>(string url, CancellationToken cancellationToken = default, List<KeyValuePair<string, object>> param = null)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var request = new RestRequest(url, Method.DELETE, _defaultDataFormat);

            param.ForEach(p => request.AddParameter(p.Key, p.Value));

            var response = await _request.ExecuteAsync<T>(request, Method.DELETE);

            return (response != null && response.IsSuccessful) ? response.Data : Option<T>.None;
        }
    }
}
