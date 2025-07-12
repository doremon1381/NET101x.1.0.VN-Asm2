using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asm2.Exceptions;
using DinkToPdf;
using DinkToPdf.Contracts;
using IdentityModel;
using IdentityService;
using MedicalModel;
using MedicalService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Asm2
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
            services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));

            // add database context and identity services here if needed
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityDbConnection")
                , b => b.MigrationsAssembly("Asm2"))
                , ServiceLifetime.Transient, ServiceLifetime.Transient);

            services.AddDbContext<MedicalDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MedicalDbConnection")
                , optionAction => optionAction.MigrationsAssembly("Asm2"))
                , ServiceLifetime.Transient, ServiceLifetime.Transient);

            var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateLifetime = true,

                ValidateIssuer = true,
                ValidIssuer = Configuration["Jwt:Issuer"],

                ValidateAudience = true,
                ValidAudience = Configuration["Jwt:Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            };
            services.AddSingleton(tokenValidationParameters);
            services.AddControllers();

            // add identity
            services.AddIdentity<IdentityModel.ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityService.IdentityDbContext>()
                .AddDefaultTokenProviders();
            services.AddTransient<IdentityService.IIdentityServices, IdentityService.IdentityServices>();
            services.AddTransient<IMedicalServices, MedicalServices>();
            // add authentication
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            // add jwt bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false; // set to true in production
                options.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Asm2", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            DataSeeding(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                // Update the Swagger endpoint address in the UseSwaggerUI call as needed.
                // For example, to change the endpoint to "/api-docs/v1/swagger.json":
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/api-docs/v1/swagger.json", "Asm2 v1"));
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Asm2 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // add built-in exception handler
            app.ConfigureBuildInExceptionHandler(loggerFactory);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void DataSeeding(IApplicationBuilder app)
        {
            app.EnsureIdentityDbCreated();
            app.EnsureMedicalDbCreated();

            app.SeedRolesAsync().GetAwaiter().GetResult();
            app.SeedUsers();
            app.SeedMedicalData();
        }
    }
}
