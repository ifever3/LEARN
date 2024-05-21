using Mediator.Net;
using Mediator.Net.Contracts;

namespace LEARN.model
{
    public class deleteStaffcommand :ICommand
    {
        public Guid Id {  get; set; }   
    }
}
