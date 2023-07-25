using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ParrotWingsApi.Context;
using ParrotWingsApi.Services.ApiService;
using ParrotWingsApi.Services.AuthorizationService;
using ParrotWingsApi.Services.DataService;
using ParrotWingsApi.Services.LoggerService;
using System.Text;

namespace ParrotWingsApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IConfiguration _configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"])),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });

        services.AddScoped<DbContext, EntityContext>();
        services.AddDbContext<EntityContext>(options => options.UseSqlServer(
            _configuration["ConnectionString"]
            .Replace("%CONTENTROOTPATH%", _configuration["CONTENTROOT"])
            ));

        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IDataServiceProvider, DataServiceProvider>();
        services.AddScoped<IApiService, ApiService>();
        services.AddSingleton<ILoggerService, LoggerService>();

        string pathLog = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
        services.AddSingleton(pathLog);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}