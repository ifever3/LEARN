namespace LEARN.db.unitofwork
{
    public interface iunitofwork
    {
        Task commitasync(CancellationToken cancellationToken = default);
    }
}
