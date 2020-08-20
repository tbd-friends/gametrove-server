using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using api.Settings;
using Azure.Core.Pipeline;
using Azure.Storage.Blobs;
using GameTrove.Api.Infrastructure;
using GameTrove.Application.Commands;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.Services;
using GameTrove.Storage;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ServicePointManager.Expect100Continue = false;

            services.AddDbContextPool<GameTrackerContext>(sql =>
                sql.UseSqlServer(Configuration.GetConnectionString("gametracker")));

            services.AddMediatR(typeof(RegisterGame).Assembly);
            services.AddHttpContextAccessor();

            services.AddTransient<IAuthenticatedMediator, AuthenticatedMediator>();
            services.AddTransient<AuthenticationService>();
            services.AddSingleton<ITokenService>(
                new DefaultTokenService(int.Parse(Configuration["settings:tokenLength"])));

            services.AddTransient(provider =>
                new BlobServiceClient(Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING"),
                    new BlobClientOptions
                    {
                        Transport = new HttpClientTransport(new HttpClient { Timeout = TimeSpan.FromSeconds(120) })
                    }));
            services.AddTransient(provider => Configuration.GetSection("images").Get<ImageSettings>());
            services.AddHttpClient<AzureDownloadClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["images:storageurl"]);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration["auth:domain"];
                options.Audience = Configuration["auth:audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("User", policy => policy.RequireRole("User"));
                options.AddPolicy("Read-Only", policy => policy.RequireRole("Read-Only"));
            });

            services.AddControllers();

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "GameTracker API";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UpdateDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/api";
                settings.DocumentPath = "/api/specification.json";
            });

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<GameTrackerContext>();

            context.Database.Migrate();
        }
    }
}
