using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace LEARN.authentication
{
    public class WiltechsAuthenticationHandler : AuthenticationHandler<WiltechsAuthenticationOptions>
    {
        public WiltechsAuthenticationHandler(IOptionsMonitor<WiltechsAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }
    }
}