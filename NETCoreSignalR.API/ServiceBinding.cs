using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Repository.Configurations;
using NETCoreSignalR.Repository.Repository;
using NETCoreSignalR.Services.Data;
using NETCoreSignalR.Services.External;
using NETCoreSignalR.Services.Pagination;
using NETCoreSignalR.Util.Configuration;
using NETCoreSignalR.Util.Security;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreSignalR.API
{
    public sealed class ServicesBinding
    {
        public ServicesBinding(IConfiguration configuration)
        {
            _config = configuration;
        }
        public IConfiguration _config { get; }

        public void BindServices(IServiceCollection services)
        {
            var apiSettings = new APISettings(_config);
            #region Services
            services.AddTransient<ILoggingService, LogService>();
            //services.AddTransient<IRestClient, RestClient>();
            services.AddTransient<IHttpConsumer, HttpConsumer>();
            services.AddTransient<ISpotifyService, SpotifyService>();
            services.AddTransient<IAuthService, AuthenticationService>((_) => new AuthenticationService(apiSettings.JWTKey));
            #endregion

            #region Helpers
            services.AddSingleton<IHashing, Hashing>();
            services.AddSingleton<IEncryption, Encryption>(_ => new Encryption(apiSettings.EncryptionKey));
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });
            #endregion


            #region Repositories
            services.AddDbContext<MyDbContext>(opt => opt.UseSqlServer(apiSettings.ConnectionString));

            services.AddScoped<IRepository<EventLog>, LogRepository>();
            #endregion

        }
    }
}
