using LEARN.db;
using LEARN.dependencyinjection;
using LEARN.model;

namespace LEARN.data
{
    public interface IStaffrepository :irepository<Staff>, IScopedDependency
    {

    }

    public class Staffrepository : repository<Staff>, IStaffrepository
    {
        public Staffrepository(appdbcontext dbContext) : base(dbContext)
        {
        }
    }
}
