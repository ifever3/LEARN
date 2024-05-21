using Microsoft.AspNetCore.Authentication;

namespace LEARN.authentication
{
    public class WiltechsAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Authority {  get; set; }
    }
}