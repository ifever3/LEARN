using Mediator.Net.Contracts;

namespace LEARN.authentication
{
    public class GetUserPermissionsRequest : IRequest
    {
        public Guid UserId { get; set; }
    }
}