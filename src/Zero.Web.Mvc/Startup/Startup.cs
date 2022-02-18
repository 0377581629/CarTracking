using System;
using System.IO;
using Abp.AspNetCore;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.AspNetZeroCore.Web.Authentication.JwtBearer;
using Abp.Castle.Logging.Log4Net;
using Abp.Hangfire;
using Abp.PlugIns;
using Castle.Facilities.Logging;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zero.Authorization;
using Zero.Configuration;
using Zero.EntityFrameworkCore;
using Zero.Identity;
using Zero.Web.Chat.SignalR;
using Zero.Web.Common;
using Zero.Web.Resources;
using Swashbuckle.AspNetCore.Swagger;
using Zero.Web.IdentityServer;
using Zero.Web.Swagger;
using Stripe;
using System.Reflection;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.AspNetCore.Mvc.Extensions;
using Hangfire.SqlServer;
using HealthChecks.UI;
using HealthChecks.UI.Client;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Zero.Web.HealthCheck;
using Owl.reCAPTCHA;
using HealthChecksUISettings = HealthChecks.UI.Configuration.Settings;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using ZERO.Web.Areas.App.Controllers;
using Zero.Web.FileManager;
using Zero.Web.FileManager.Interfaces;

namespace Zero.Web.Startup
{
    public class Startup
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
            _hostingEnvironment = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            LoadGlobalConfig();

            // MVC
            services.AddControllersWithViews(options => { options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute()); })
#if DEBUG
                .AddRazorRuntimeCompilation()
#endif
                .AddNewtonsoftJson();
            if (bool.Parse(_appConfiguration["KestrelServer:IsEnabled"]))
            {
                ConfigureKestrel(services);
            }

            IdentityRegistrar.Register(services);

            //Identity server
            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                IdentityServerRegistrar.Register(services, _appConfiguration, options =>
                    options.UserInteraction = new UserInteractionOptions()
                    {
                        LoginUrl = "/Account/Login",
                        LogoutUrl = "/Account/LogOut",
                        ErrorUrl = "/Error"
                    });
            }

            AuthConfigurer.Configure(services, _appConfiguration);

            if (WebConsts.SwaggerUiEnabled)
            {
                //Swagger - Enable this line and the related lines in Configure method to enable swagger UI
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Zero API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.ParameterFilter<SwaggerEnumParameterFilter>();
                    options.SchemaFilter<SwaggerEnumSchemaFilter>();
                    options.OperationFilter<SwaggerOperationIdFilter>();
                    options.OperationFilter<SwaggerOperationFilter>();
                    options.CustomDefaultSchemaIdSelector();
                    options.CustomSchemaIds(type => type.ToString());
                }).AddSwaggerGenNewtonsoftSupport();
            }

            //Recaptcha
            services.AddreCAPTCHAV3(x =>
            {
                x.SiteKey = _appConfiguration["Recaptcha:SiteKey"];
                x.SiteSecret = _appConfiguration["Recaptcha:SecretKey"];
            });

            if (WebConsts.HangfireDashboardEnabled)
            {
                //Hangfire (Enable to use Hangfire instead of default job manager)
                services.AddHangfire(config => { config.UseSqlServerStorage(_appConfiguration.GetConnectionString("Default")); });
                JobStorage.Current = new SqlServerStorage(_appConfiguration.GetConnectionString("Default"));
            }

            services.AddScoped<IWebResourceManager, WebResourceManager>();
            services.AddSingleton<IContentBrowser, ContentBrowser>();
            
            services.AddSignalR();

            services.Configure<SecurityStampValidatorOptions>(options => { options.ValidationInterval = TimeSpan.Zero; });

            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
            {
                services.AddAbpZeroHealthCheck();

                var healthCheckUISection = _appConfiguration.GetSection("HealthChecks")?.GetSection("HealthChecksUI");

                if (bool.Parse(healthCheckUISection["HealthChecksUIEnabled"]))
                {
                    services.Configure<HealthChecksUISettings>(settings => { healthCheckUISection.Bind(settings, c => c.BindNonPublicProperties = true); });

                    services.AddHealthChecksUI()
                        .AddInMemoryStorage();
                }
            }

            services.Configure<RazorViewEngineOptions>(options => { options.ViewLocationExpanders.Add(new RazorViewLocationExpander()); });

            #region Telerik Report Rest service Configuration

            services.AddControllers();

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
                options.MaxRequestBodySize = int.MaxValue;
            });

            // Configure dependencies for ReportsController.
            services.TryAddSingleton<IReportServiceConfiguration>(sp =>
                new ReportServiceConfiguration
                {
                    HostAppId = $"ReportingCore5App-{Guid.NewGuid()}",
                    Storage = new FileStorage(),
                    ReportSourceResolver = new UriReportSourceResolver(Path.Combine(sp.GetService<IWebHostEnvironment>()?.ContentRootPath ?? string.Empty, "Reports")),
                });

            #endregion
            
            //Configure Abp and Dependency Injection
            return services.AddAbp<ZeroWebMvcModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig(_hostingEnvironment.IsDevelopment()
                        ? "log4net.config"
                        : "log4net.Production.config")
                );

                options.PlugInSources.AddFolder(Path.Combine(_hostingEnvironment.WebRootPath, "Plugins"),
                    SearchOption.AllDirectories);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //Initializes ABP framework.
            app.UseAbp(options =>
            {
                options.UseAbpRequestLocalization = false; //used below: UseAbpRequestLocalization
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("~/Error?statusCode={0}");
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();

            if (bool.Parse(_appConfiguration["Authentication:JwtBearer:IsEnabled"]))
            {
                app.UseJwtTokenMiddleware();
            }

            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                app.UseJwtTokenMiddleware("IdentityBearer");
                app.UseIdentityServer();
            }

            app.UseAuthorization();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                if (scope.ServiceProvider.GetService<DatabaseCheckHelper>()
                    .Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    app.UseAbpRequestLocalization();
                }
            }

            if (WebConsts.HangfireDashboardEnabled)
            {
                //Hangfire dashboard & server (Enable to use Hangfire instead of default job manager)
                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[]
                    {
                        new AbpHangfireAuthorizationFilter(AppPermissions.Pages_Administration_HangfireDashboard)
                    }
                });
                app.UseHangfireServer();
            }

            if (bool.Parse(_appConfiguration["Payment:Stripe:IsActive"]))
            {
                StripeConfiguration.ApiKey = _appConfiguration["Payment:Stripe:SecretKey"];
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");
                endpoints.MapHub<ChatHub>("/signalr-chat");

                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

                if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
                {
                    endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
                }

                app.ApplicationServices.GetRequiredService<IAbpAspNetCoreConfiguration>().EndpointConfiguration.ConfigureAllEndpoints(endpoints);
            });

            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
            {
                if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksUI:HealthChecksUIEnabled"]))
                {
                    app.UseHealthChecksUI();
                }
            }

            if (WebConsts.SwaggerUiEnabled)
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger();
                //Enable middleware to serve swagger - ui assets(HTML, JS, CSS etc.)
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(_appConfiguration["App:SwaggerEndPoint"], "Zero API V1");
                    options.IndexStream = () => Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("Zero.Web.wwwroot.swagger.ui.index.html");
                    options.InjectBaseUrl(_appConfiguration["App:WebSiteRootAddress"]);
                }); //URL: /swagger
            }
        }

        private void ConfigureKestrel(IServiceCollection services)
        {
            services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
            {
                options.Listen(new System.Net.IPEndPoint(System.Net.IPAddress.Any, 443),
                    listenOptions =>
                    {
                        var certPassword = _appConfiguration.GetValue<string>("Kestrel:Certificates:Default:Password");
                        var certPath = _appConfiguration.GetValue<string>("Kestrel:Certificates:Default:Path");
                        var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath,
                            certPassword);
                        listenOptions.UseHttps(new HttpsConnectionAdapterOptions()
                        {
                            ServerCertificate = cert
                        });
                    });
            });
        }

        private void LoadGlobalConfig()
        {
            #region System

            ZeroConst.MultiTenancyEnabled = bool.Parse(_appConfiguration["GlobalConfig:AllowMultiTenancy"]);
            SystemConfig.DisableMailService = bool.Parse(_appConfiguration["GlobalConfig:DisableMailService"]);
            SystemConfig.LogIndex = _appConfiguration["GlobalConfig:LogIndex"];
            if (!string.IsNullOrEmpty(_appConfiguration["GlobalConfig:DefaultPassword"]))
                SystemConfig.DefaultPassword = _appConfiguration["GlobalConfig:DefaultPassword"];

            SystemConfig.UseFileServer = bool.Parse(_appConfiguration["FileServer:IsActive"]);
            if (SystemConfig.UseFileServer)
            {
                SystemConfig.MinioEndPoint = _appConfiguration["FileServer:EndPoint"];
                SystemConfig.MinioRootBucketName = _appConfiguration["FileServer:RootBucketName"];
                SystemConfig.MinioAccessKey = _appConfiguration["FileServer:AccessKey"];
                SystemConfig.MinioSecretKey = _appConfiguration["FileServer:SecretKey"];
                SystemConfig.MinioRegion = _appConfiguration["FileServer:Region"];
            }

            SystemConfig.UseEmailValidation = bool.Parse(_appConfiguration["EmailValidationServer:IsActive"]);
            if (SystemConfig.UseEmailValidation)
            {
                SystemConfig.EmailValidationEndPoint = _appConfiguration["EmailValidationServer:EndPoint"];
                SystemConfig.EmailValidationApiKey = _appConfiguration["EmailValidationServer:ApiKey"];
                
                AbstractApi.Config.IsActive = SystemConfig.UseEmailValidation;
                AbstractApi.Config.Endpoint = SystemConfig.EmailValidationEndPoint;
                AbstractApi.Config.ApiKey = SystemConfig.EmailValidationApiKey;
            }
            
            #endregion

            #region Layout

            GlobalConfig.AppName = _appConfiguration["GlobalConfig:AppName"];
            GlobalConfig.AppFooter = _appConfiguration["GlobalConfig:AppFooter"];
            GlobalConfig.AppDescription = _appConfiguration["GlobalConfig:AppDescription"];
            GlobalConfig.AppKeyword = _appConfiguration["GlobalConfig:AppKeyword"];
            GlobalConfig.AppAuthor = _appConfiguration["GlobalConfig:AppAuthor"];

            GlobalConfig.AppFaviconName = _appConfiguration["GlobalConfig:AppFaviconName"];
            GlobalConfig.AppDefaultLogo = _appConfiguration["GlobalConfig:AppDefaultLogo"];

            #endregion

            #region Account Layout

            GlobalConfig.AppLoginTitle = _appConfiguration["LoginPage:AppLoginTitle"];
            GlobalConfig.AppLoginSubtitle = _appConfiguration["LoginPage:AppLoginSubtitle"];

            GlobalConfig.AppDefaultLogoLogin = _appConfiguration["LoginPage:AppDefaultLogoLogin"];
            GlobalConfig.AppDefaultBackgroundLogin = _appConfiguration["LoginPage:AppDefaultBackgroundLogin"];
            GlobalConfig.AppDefaultBackgroundLoginColor = _appConfiguration["LoginPage:AppDefaultBackgroundLoginColor"];
            GlobalConfig.AppDefaultBackgroundSize = _appConfiguration["LoginPage:AppDefaultBackgroundSize"];

            #endregion

            #region Menu Logo

            GlobalConfig.UseMenuLogo = bool.Parse(_appConfiguration["AppMenuLogo:IsActive"]);
            GlobalConfig.AppDefaultMenuLogo = _appConfiguration["AppMenuLogo:Url"];
            if (!string.IsNullOrEmpty(_appConfiguration["GlobalConfig:DefaultImage"]))
                GlobalConfig.DefaultImageUrl = _appConfiguration["GlobalConfig:DefaultImage"];

            #endregion

            #region Upload , Import

            GlobalConfig.AppPhysPath = _hostingEnvironment.WebRootPath;
            GlobalConfig.UploadPath = _appConfiguration["GlobalConfig:UploadPath"];
            GlobalConfig.MaxUploadFileSize = Convert.ToUInt32(_appConfiguration["GlobalConfig:MaxUploadFileSize"]);
            GlobalConfig.ImportTemplatePath = _appConfiguration["GlobalConfig:ImportTemplatePath"];
            if (!string.IsNullOrEmpty(_appConfiguration["GlobalConfig:ImportSampleFolders"]))
                GlobalConfig.ImportSampleFolders = _appConfiguration["GlobalConfig:ImportSampleFolders"];

            #endregion
        }
    }
}