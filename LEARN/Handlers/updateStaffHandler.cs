using AutoMapper;
using LEARN.data;
using LEARN.model;
using Mediator.Net.Context;
using Mediator.Net.Contracts;

namespace LEARN.Handlers
{
    public class updateStaffHandler : ICommandHandler<updateStaffcommand, updateStaffresponse>
    {

        private readonly IStaffrepository _staffrepository;
        private readonly IMapper _mapper;

        public updateStaffHandler(IStaffrepository staffrepository, IMapper mapper)
        {
            _staffrepository = staffrepository;
            _mapper = mapper;
        }

        public async Task<updateStaffresponse> Handle(IReceiveContext<updateStaffcommand> context, CancellationToken cancellationToken)
        {
            var entity = await _staffrepository.GetByIdAsync(context.Message.Id, cancellationToken);
            entity = _mapper.Map(context.Message,entity);
            await _staffrepository.UpdateAsync(entity, cancellationToken);
            return new updateStaffresponse
            {
                Name= entity.name,
                Major= entity.major
            };
        }
    }
}
