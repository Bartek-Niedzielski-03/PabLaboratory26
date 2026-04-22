using AppCore.Authorization;
using AppCore.Repositories;
using AppCore.Services;
using AppCore.Users;
using Infrastructure.EntityFramework.Context;
using Infrastructure.EntityFramework.Entities;
using Infrastructure.EntityFramework.Repositories;
using Infrastructure.EntityFramework.UnitOfWork;
using Infrastructure.Memory;
using Infrastructure.Security;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class ContactsInfrastructureModule
{
    public static IServiceCollection AddContactsEfModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //DbContext
        services.AddDbContext<ContactsDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("CrmDb")));

        //Repositories - EF
        services.AddScoped<IPersonRepository, EfPersonRepository>();
        services.AddScoped<ICompanyRepository, EfCompanyRepository>();
        services.AddScoped<IOrganizationRepository, EfOrganizationRepository>();

        //unit of work
        services.AddScoped<IContactUnitOfWork, EfContactsUnitOfWork>();

        //identity
        services.AddIdentity<CrmUser, CrmRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<ContactsDbContext>()
            .AddDefaultTokenProviders();

        //services
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDataSeeder, PersonDbSeeder>();

        return services;
    }
    
    public static IServiceCollection AddJwt(
    this IServiceCollection services,
    JwtSettings jwtOptions)
{
    services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = jwtOptions.GetSymmetricKey(),
                ClockSkew = TimeSpan.Zero
            };
        });

    services.AddAuthorization(options =>
    {
        options.AddPolicy(CrmPolicies.AdminOnly.ToString(), policy =>
            policy.RequireRole(UserRole.Administrator.ToString()));

        options.AddPolicy(CrmPolicies.SalesAccess.ToString(), policy =>
            policy.RequireRole(
                UserRole.Administrator.ToString(),
                UserRole.SalesManager.ToString(),
                UserRole.Salesperson.ToString()));

        options.AddPolicy(CrmPolicies.SalesManagerAccess.ToString(), policy =>
            policy.RequireRole(
                UserRole.Administrator.ToString(),
                UserRole.SalesManager.ToString()));

        options.AddPolicy(CrmPolicies.SupportAccess.ToString(), policy =>
            policy.RequireRole(
                UserRole.Administrator.ToString(),
                UserRole.SupportAgent.ToString()));

        options.AddPolicy(CrmPolicies.ReadOnlyAccess.ToString(), policy =>
            policy.RequireRole(
                UserRole.Administrator.ToString(),
                UserRole.SalesManager.ToString(),
                UserRole.Salesperson.ToString(),
                UserRole.SupportAgent.ToString(),
                UserRole.ReadOnly.ToString()));

        options.AddPolicy(CrmPolicies.ActiveUser.ToString(), policy =>
            policy
                .RequireAuthenticatedUser()
                .RequireClaim("status", SystemUserStatus.Active.ToString()));

        options.AddPolicy(CrmPolicies.SalesDepartment.ToString(), policy =>
            policy.RequireClaim("department", "Sales"));

        options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });

    return services;
}

    //pozostawiamy możliwość użycia implementacji pamięciowej
    public static IServiceCollection AddContactsMemoryModule(
        this IServiceCollection services)
    {
        services.AddSingleton<IPersonRepository, MemoryPersonRepository>();
        services.AddSingleton<ICompanyRepository, MemoryCompanyRepository>();
        services.AddSingleton<IOrganizationRepository, MemoryOrganizationRepository>();
        services.AddSingleton<IContactUnitOfWork, MemoryContactUnitOfWork>();
        services.AddSingleton<IPersonService, MemoryPersonService>();

        return services;
    }
}