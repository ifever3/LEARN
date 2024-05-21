using LEARN.data;
using LEARN.model;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace LEARN.Handlers
{
    public class deleteStaffHandler : ICommandHandler<deleteStaffcommand>
    {
        private readonly IStaffrepository _repository;
        
        public deleteStaffHandler(IStaffrepository irepository)
        {
            _repository = irepository;
        }
        public async Task Handle(IReceiveContext<deleteStaffcommand> context, CancellationToken cancellationToken)
        {

            var staff = await _repository.GetByIdAsync(context.Message.Id, cancellationToken)
           .ConfigureAwait(false);

            await _repository.DeleteAsync(staff, cancellationToken).ConfigureAwait(false);
        }

    }
}
