using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace LEARN.authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IMemoryCache _memoryCache;
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock,IMemoryCache memoryCache) : base(options, logger, encoder, clock)
        {
            _memoryCache =  memoryCache;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (!Request.Headers.ContainsKey("x-api-key")) return AuthenticateResult.NoResult();

                var apiKey = Request.Headers["x-api-key"].ToString();
                if (_memoryCache.TryGetValue($"api_key_{apiKey}", out string userInfo))
                {
                    // 查询配置的ApiKey 是否对应的上
                }



                return new HandleRequestResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}