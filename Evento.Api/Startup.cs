﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Evento.Core.Domain;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Evento.Infrastructure.AutoMapper;
using Evento.Core.Repositories;
using Evento.Infrastructure.Repositories;
using Evento.Infrastructure.Services;
using Newtonsoft.Json;
using Evento.Infrastructure.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Evento.Infrastructure.Settings;
using Evento.Infrastructure.Services.User.JwtToken;
using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Evento.Infrastructure.IoC.Modules;
using Evento.Api.Framework;
using Swashbuckle.AspNetCore.Swagger;

namespace Evento.Api
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "Origins";
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Wstrzykiwanie zależności
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IJwtHandler, JwtHandler>();
            // Poprawienie formatowania skłądni json
            services.AddMvc()
                .AddJsonOptions(x => x.SerializerSettings.Formatting = Formatting.Indented);

            // Dodanie policy
            services.AddAuthorization(x => x.AddPolicy("HasAdminRole", p => p.RequireRole("admin")));

            // Konfiguracja AutoMappera
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Łączenie się z bazą danych MS SQL
            services.AddDbContext<DataBaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DevConnection"), b => b.MigrationsAssembly("Evento.Api"))
            );

            // Konfiguracja Jwt token
            // https://go.microsoft.com/fwlink/?linkid=845470
            // https://bit.ly/2Rg7UIj

            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SigningKey"])),
                    ValidateIssuer = false,     // Podmiot zdolny do wystawienia tokenu. UWAGA: ValidateIssuer != ValidIssuer
                    ValidateAudience = false    // Strony mogące kożystać z serwisu. UWAGA: ValidateAudience != ValidAudience
                };
            });

            // Konfiguracja Swagger'a
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Evento service",
                    Description = "Evento service",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Talking Dotnet", Email = "contact@talkingdotnet.com", Url = "www.talkingdotnet.com" }
                });
                c.CustomSchemaIds(x => x.FullName);
            });

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                config =>
                {
                    config
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(jsonOptions =>
            {
                jsonOptions.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            // Konfiguracja AutoFac'a
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule<CommandModule>();
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            else
                app.UseHsts();

            app.UseCors(MyAllowSpecificOrigins);
            app.UserExceptionHandler();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}
