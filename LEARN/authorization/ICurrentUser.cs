using LEARN.dependencyinjection;
using System.Security.Claims;

namespace LEARN.authorization
{
    public interface ICurrentUser : IScopedDependency
    {
        Guid Id { get; }
        public string UserName { get; }
    }

    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Id
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                    throw new ApplicationException("HttpContext is not available");

                var idClaim =
                    _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier);

                return Guid.Parse(idClaim.Value);
            }
        }

        public string UserName
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                    throw new ApplicationException("HttpContext is not available");

                var nameClaim =
                    _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.Name);

                return nameClaim.Value;
            }
        }
    }
}