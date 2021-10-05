using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NETCoreSignalR.Domain.Entities;
using NETCoreSignalR.Domain.Interfaces;
using NETCoreSignalR.Repository.Configurations;
using NETCoreSignalR.Repository.Repository;
using NETCoreSignalR.Services.Data;
using NETCoreSignalR.Services.External;
using NETCoreSignalR.Services.External.PokeAPI;
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
    public static class ServicesBinding
    {

        public static void BindServices(this IServiceCollection services, IConfiguration config)
        {
            var apiSettings = new APISettings(config);
            #region Services
            services.AddTransient<ILoggingService, LogService>();

            services.AddTransient<IRestClient, RestClient>();
            services.AddScoped<IHttpConsumer, HttpConsumer>((sp) => new HttpConsumer(sp.GetRequiredService<IRestClient>(), DataFormat.Json));
            services.AddScoped<IPokeAPIConsumer, PokeAPIConsumer>();
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
            services.AddDbContext<MyDbContext>(opt => 
                opt.UseSqlServer(apiSettings.ConnectionString,
                    options => { 
                        options.MigrationsAssembly(typeof(MyDbContext).Assembly.FullName); 
                    }));

            services.AddScoped<IRepository<EventLog>, LogRepository>();
            #endregion

        }
    }
}
