using Mediator.Net.Contracts;

namespace LEARN.model
{
    public class createStaffcommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Major { get; set; }

    }

    public class createStaffresponse : IResponse
    {
        public Guid Id { get; set; }
    }
}
