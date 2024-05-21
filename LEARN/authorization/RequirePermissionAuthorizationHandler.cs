using LEARN.authorization;
using Mediator.Net;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace LEARN.authentication
{
    internal class RequirePermissionAuthorizationHandler : AuthorizationHandler<RequirePermissionRequirement>
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUser _currentUser;

        public RequirePermissionAuthorizationHandler(ICurrentUser currentUser, IMediator mediator)
        {
            _currentUser = currentUser;
            _mediator = mediator;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RequirePermissionRequirement requirement)
        {
            if (context.User.Identity is not { IsAuthenticated: true })
            {
                context.Fail();
                return;
            }

            var response = await _mediator.RequestAsync<GetUserPermissionsRequest, BaseResponse<List<UserPermissionDto>>>(
                new GetUserPermissionsRequest
                {
                    UserId = _currentUser.Id,
                });

            if (!await AuthorizeAsync(requirement.Permission, response.Data))
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }

        private Task<bool> AuthorizeAsync(string permission, List<UserPermissionDto> permissions)
        {
            if (Debugger.IsAttached)
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(permissions.Any(x => x.Permission == permission));
        }
    }
}