using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NETCoreSignalR.Services.External;
using NETCoreSignalR.Util.Configuration;
using Newtonsoft.Json;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NETCoreSignalR.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddDirectoryBrowser();


            #region Setting json handler
            services.AddControllers()
                .AddJsonOptions(ops =>
                {
                    ops.JsonSerializerOptions.IgnoreReadOnlyProperties = false;
                    ops.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    ops.JsonSerializerOptions.IgnoreNullValues = false;
                    ops.JsonSerializerOptions.WriteIndented = true;
                    ops.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    ops.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    ops.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    ops.JsonSerializerOptions.MaxDepth = 64;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            #endregion

            #region API Versioning
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(2, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("x-api-version"), new QueryStringApiVersionReader("api-version"));
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
            #endregion

            #region Swagger documentation setup
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerConfiguration>();
            });
            #endregion

            #region JWT token bearer and authentication setup

            var appSettings = new APISettings(Configuration);
            byte[] tokenKeyBytes = Encoding.ASCII.GetBytes(appSettings.JWTKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.IncludeErrorDetails = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKeyBytes),
                    ValidateIssuer = true,
                    ValidIssuer = "NET Core API",
                    ValidateAudience = false,
                    ValidateTokenReplay = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            #endregion

            #region Setting SignalR support

            services.AddSignalR();
            #endregion

            #region CORS Setup
            services.AddCors(options =>
            {
                options.AddPolicy("WebApp",
                    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

                //options.AddPolicy("WebApp",
                //    builder => builder.WithOrigins("http://example.com").AllowAnyHeader().AllowAnyMethod());
            });
            #endregion

            ServicesBinding servicesBinder = new ServicesBinding(Configuration);
            servicesBinder.BindServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiVersioningProvider)
        {
            var apiConfig = new APISettings(Configuration);
            #region Setting up exception handling
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(c => c.Run(async context =>
                {
                    var exception = context.Features
                        .Get<IExceptionHandlerPathFeature>()
                        .Error;
                    var result = JsonConvert.SerializeObject(new { message = exception.Message, stacktrace = exception.StackTrace });
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(result);
                }));
            }
            #endregion

            #region setting up logging 

            Log.Logger = new LoggerConfiguration()
                      .Enrich.FromLogContext()
                      .WriteTo.MSSqlServer(apiConfig.ConnectionString,
                          autoCreateSqlTable: true,
                          restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
                          tableName: Configuration["Logging:Table"])
                      .CreateLogger();


            #endregion

            #region Setting up swagger UI with versioning
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                foreach (var description in apiVersioningProvider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
            #endregion

            #region Signal R Setup

            app.UseSignalR(route => { route.MapHub<ChatHub>("/user-hub"); });
            #endregion
            #region Setting up Azure Keyvault

            if (env.IsProduction())
            {
                var sp = app.ApplicationServices;
                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                   .AddAzureKeyVault(new AzureKeyVaultConfigurationOptions(apiConfig.KeyVaultURI, apiConfig.KeyVaultClientId, apiConfig.KeyVaultKey));

                Configuration = builder.Build();
            }
            #endregion

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
