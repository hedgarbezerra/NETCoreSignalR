using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETCoreSignalR.Domain.Interfaces
{
    public interface IHttpConsumer
    {
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
}
