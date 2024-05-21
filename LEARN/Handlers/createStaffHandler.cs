using AutoMapper;
using LEARN.data;
using LEARN.model;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Windows.Input;

namespace LEARN.Handlers
{
    public class createStaffHandler : ICommandHandler<createStaffcommand, createStaffresponse>
    {
        private readonly IStaffrepository _staffrepository;
       private readonly IMapper _mapper;

        public createStaffHandler(IStaffrepository staffrepository, IMapper mapper)
        {
            _staffrepository = staffrepository;
            _mapper = mapper;
        }

        public async Task<createStaffresponse> Handle(IReceiveContext<createStaffcommand> context, CancellationToken cancellationToken)
        {
            var command =context.Message;
            var sta = _mapper.Map<Staff>(command);
            await _staffrepository.InsertAsync(sta, cancellationToken).ConfigureAwait(false);

            return new createStaffresponse
            {          
                Id = sta.id,
        };
        }
    }
}
