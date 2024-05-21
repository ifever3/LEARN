using Microsoft.AspNetCore.Authorization;

namespace LEARN.authentication
{
    public class RequirePermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public RequirePermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}