using Microsoft.AspNetCore.Builder;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Evento.Infrastructure.Settings;
using Evento.Infrastructure.Services.User.JwtToken;

namespace Evento.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // Wstrzykiwanie zależności
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IJwtHandler, JwtHandler>();
            // Poprawienie formatowania skłądni json
            services.AddMvc()
                .AddJsonOptions(x => x.SerializerSettings.Formatting = Formatting.Indented);

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
            // https://youtu.be/yH4GhmTPf68
            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));

            services.AddIdentity<IdentityUser, IdentityRole>( option => {
                option.Password.RequireDigit = false;           // wymagana cyfra
                option.Password.RequiredLength = 6;             // wymagana długość
                option.Password.RequireNonAlphanumeric = false; // wymagane znaki alfanumeryczne
                option.Password.RequireUppercase = false;       // wymagane wielkie litery
                option.Password.RequireLowercase = false;       // wymagane małe litery
            }).AddEntityFrameworkStores<DataBaseContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option => {
                option.SaveToken = true;
                option.RequireHttpsMetadata = true;
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateActor = true,
                    ValidAudience = Configuration["Jet:Site"],  // Strony mogące korzystać z tego serwera
                    ValidIssuer = Configuration["Jwt:Site"],    // Podmiot zdolny do wystawienia tokenu
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SigningKey"]))
                };
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseAuthentication();
        }
    }
}
