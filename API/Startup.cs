using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using API.Model.Context;
using API.Model.Repositories;
using API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;

namespace API
{
    public class Startup
    {

        public IConfiguration AppConfig { get; set; }

        public Startup(IConfiguration config)
        {
            AppConfig = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
            });
            services.AddCors();
            ConfigureDatabase(services);
            ConfigureJwt(services);
            ConfigureRepositories(services);
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            var connection = AppConfig.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDatabaseContext>(options => options.UseSqlServer(connection));
        }

        private void ConfigureJwt(IServiceCollection services)
        {
            var issuer = AppConfig.GetValue<string>("JWT:Issuer");
            var key = AppConfig.GetValue<string>("JWT:Key");
            var jwtConfig = new JwtConfig(issuer, key);

            services.AddSingleton<JwtConfig>(jwtConfig);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfig.Issuer,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtConfig.GetSymmetricSecurityKey(),

                        ValidateLifetime = false,
                        ValidateAudience = false
                    };
                });
            services.AddAuthorization();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            var repositories = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetCustomAttributes<RepositoryAttribute>().Any());
            foreach (var repository in repositories)
            {
                services.AddScoped(repository);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDatabaseContext dbContext)
        {
            dbContext.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            var allowedOrigins = AppConfig.GetValue<string>("AllowedOrigins");
            app.UseCors(builder =>
            {
                builder.WithOrigins(allowedOrigins);
                builder.AllowCredentials();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
