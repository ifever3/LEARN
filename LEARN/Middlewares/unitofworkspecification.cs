using System.Runtime.ExceptionServices;
using LEARN.db.unitofwork;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;


namespace LEARN.Middlewares;

public class UnitOfWorkSpecification<TContext> : IPipeSpecification<TContext>
    where TContext : IContext<IMessage>
{
    private readonly iunitofwork _iunitofwork;

    public UnitOfWorkSpecification(iunitofwork unitOfWork)
    {
        _iunitofwork = unitOfWork;
    }

    public bool ShouldExecute(TContext context, CancellationToken cancellationToken)
    {
        return context.Message is ICommand or IEvent;
    }

    public Task BeforeExecute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Execute(TContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task AfterExecute(TContext context, CancellationToken cancellationToken)
    {
        return ShouldExecute(context, cancellationToken)
            ? _iunitofwork.commitasync(cancellationToken)
            : Task.CompletedTask;
    }

    public Task OnException(Exception ex, TContext context)
    {
        ExceptionDispatchInfo.Capture(ex).Throw();
        return Task.CompletedTask;
    }
}
