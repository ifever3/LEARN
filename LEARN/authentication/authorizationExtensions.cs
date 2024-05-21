using Autofac.Core;
//using LEARN.authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

namespace LEARN.authentication
{
    public static class authorizationExtensions
    {
        public static void addcustomauthentication(this IServiceCollection services,IConfiguration configuration)
        {
            var secret = Encoding.ASCII.GetBytes(configuration.GetValue<string>("Authentication:Self:JwtSecret"));

            services.Configure<IdentityOptions>(options =>
            {
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                })
                .AddScheme<WiltechsAuthenticationOptions, WiltechsAuthenticationHandler>(
                AuthenticationSchemeConstants.WiltechsAuthenticationScheme,
                options => options.Authority = configuration["Authentication:Wiltechs:Authority"])
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                AuthenticationSchemeConstants.ApiKeyAuthenticationScheme, _ => { });

            //services.AddAuthorization(options =>
            //{
            //    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
            //        JwtBearerDefaults.AuthenticationScheme,
            //        AuthenticationSchemeConstants.WiltechsAuthenticationScheme,
            //        AuthenticationSchemeConstants.ApiKeyAuthenticationScheme);

            //    defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

            //    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            //});

            //services.AddScoped<ICurrentUser, CurrentUser>();
            //services.AddSingleton<IAuthorizationPolicyProvider, RequirePermissionPolicyProvider>();
            //services.AddScoped<IAuthorizationHandler, RequirePermissionAuthorizationHandler>();
        }                                
    }
}
