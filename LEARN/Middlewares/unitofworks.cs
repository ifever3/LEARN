using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Mediator.Net;
using LEARN.db.unitofwork;

namespace LEARN.Middlewares
{
    public static class unitofworks
    {
        public static void UseUnitOfWork<TContext>(this IPipeConfigurator<TContext> configurator)
       where TContext : IContext<IMessage>
        {
            if (configurator.DependencyScope == null)
                throw new DependencyScopeNotConfiguredException("IDependencyScope is not configured.");

            var uow = configurator.DependencyScope.Resolve<iunitofwork>();

            configurator.AddPipeSpecification(new UnitOfWorkSpecification<TContext>(uow));
        }
    }
}
