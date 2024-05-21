using LEARN.data;
using LEARN.model;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LEARN.Handlers
{
    public class getStaffHandler : IRequestHandler<getStaffrequest, getStaffresponse<Staff>>
    {
        private readonly IStaffrepository _staffrepository;


        public getStaffHandler(IStaffrepository staffrepository)
        {
            _staffrepository = staffrepository;
    
        }
        public async Task<getStaffresponse<Staff>> Handle(IReceiveContext<getStaffrequest> context, CancellationToken cancellationToken)
        {
            var query = _staffrepository.Table.AsQueryable();
            
           // var k = _redishelp.GetValue("key");

            return new getStaffresponse<Staff>(query.Select(s => new Staff
            {
                id = s.id,
                name = s.name,
                major = s.major
            }).Where(x => x.major == context.Message.Major).ToList());

            throw new NotImplementedException();
        }
    }
}
