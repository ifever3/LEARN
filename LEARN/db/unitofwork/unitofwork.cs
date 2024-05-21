using LEARN.db;

namespace LEARN.db.unitofwork
{
    public class unitofwork : iunitofwork
    {
        private readonly appdbcontext _context;

        public unitofwork(appdbcontext context)
        {
            _context = context;
        }

        public Task commitasync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
