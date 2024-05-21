using LEARN.db;
using LEARN.model;
using Mediator.Net;
using Shouldly;

namespace LEARN.Test
{
    public class stafftest : IntegrationTestBase
    {
        public stafftest(integrationfixture fixture) : base(fixture) { }

        [Fact]
        public async Task shouldcreatestaff()
        {
           await  Run<IMediator, appdbcontext>(async (mediator, context) =>
            {
                var command = new createStaffcommand()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    Major = "test"
                };
                var response = await mediator.SendAsync<createStaffcommand, createStaffresponse>(command);

                var staff = await context.Set<Staff>().FindAsync(response.Id);
                staff.ShouldNotBeNull();
                staff.name.ShouldBe(command.Name);
                staff.major.ShouldBe(command.Major);
            });
        }
    }
}