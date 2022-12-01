using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WantApp.Domain.Repositories;
using WantApp.InfraData.Dapper.Employees;
using WantApp.InfraData.Data;
using WantApp.InfraData.Repositories;

namespace WantApp.API.DI;

public static class DependencyInjection
{
    public static void AddDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<QueryEmployees>();

        builder.Services.AddDbContext<ApplicationDbContext>(x =>
        {
            x.UseSqlServer(builder.Configuration["DefaultConnection"]);
        });

        builder.Services.AddIdentity<IdentityUser, IdentityRole>(optins =>
        {
            optins.Password.RequireNonAlphanumeric = true;
            optins.Password.RequireDigit = true;
            optins.Password.RequireUppercase = true;
            optins.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddAuthorization(opt =>
        {
            opt.FallbackPolicy = new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
        });
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateActor = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Issuer"],
                ValidAudience = builder.Configuration["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration["SecretKey"]))
            };
        });
    }
}
