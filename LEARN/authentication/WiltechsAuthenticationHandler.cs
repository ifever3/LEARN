using LEARN.cache;
using LEARN.data;
using LEARN.model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace LEARN.authentication
{
    public class WiltechsAuthenticationHandler : AuthenticationHandler<WiltechsAuthenticationOptions>
    {
             private readonly ICacheManager _cacheManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly irepository<Staff> _staffRepository;


        public WiltechsAuthenticationHandler(IOptionsMonitor<WiltechsAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, IHttpClientFactory clientFactory, ICacheManager cacheManager,
            irepository<Staff> staffRepository) : base(
            options, logger, encoder, clock)
        {
            _clientFactory = clientFactory;
            _cacheManager = cacheManager;
            _staffRepository = staffRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.NoResult();

            var authorization = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authorization) || !authorization.StartsWith("Bearer"))
                return AuthenticateResult.NoResult();

            //If it is jwt, use other authentication handler
            if (IsJwtToken(authorization))
            {
                return AuthenticateResult.NoResult();
            }

            try
            {
                var userInfo = await _cacheManager.GetOrAddAsync(authorization, async () =>
                {
                    var client = _clientFactory.CreateClient();
                    client.BaseAddress = new Uri(Options.Authority);
                    client.DefaultRequestHeaders.Add("Authorization", authorization);

                    var wiltechsUserInfo = await client.GetFromJsonAsync<WiltechsUserInfo>("");

                    if (wiltechsUserInfo != null && wiltechsUserInfo.UserId != Guid.Empty)
                    {
                        return wiltechsUserInfo;
                    }

                    return null;
                }, TimeSpan.FromDays(30));

                if (userInfo == null)
                    return AuthenticateResult.NoResult();

                var thisStaffInfo = await _staffRepository.SingleOrDefaultAsync(x =>
                    x.id == userInfo.UserId && x.name == userInfo.UserName);
                if (thisStaffInfo == null)
                    return AuthenticateResult.NoResult();

                var identity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserId.ToString()),
                new Claim(ClaimTypes.Name, userInfo.UserName)
            }, AuthenticationSchemeConstants.WiltechsAuthenticationScheme);

                var claimsPrincipal = new ClaimsPrincipal(identity);

                var authenticationTicket = new AuthenticationTicket(claimsPrincipal,
                    new AuthenticationProperties { IsPersistent = false },
                    Scheme.Name);

                Request.HttpContext.User = claimsPrincipal;

                return AuthenticateResult.Success(authenticationTicket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private static bool IsJwtToken(string authorization)
        {
            var token = authorization.Replace("Bearer ", "");

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.CanReadToken(token);
        }
    }
    
}