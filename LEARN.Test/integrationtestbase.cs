using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEARN.Test
{
    [Collection("Sequential")]
    public class IntegrationTestBase : IAsyncLifetime
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly Func<Task> _resetDatabase;

        protected IntegrationTestBase(integrationfixture fixture)
        {
            _resetDatabase = fixture.ResetDatabase;
            _lifetimeScope = fixture.LifetimeScope;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return _resetDatabase();
        }

        protected async Task Run<T>(Func<T, Task> action, Action<ContainerBuilder> extraRegistration = null)
        {
            var dependency = extraRegistration != null
                ? _lifetimeScope.BeginLifetimeScope(extraRegistration).Resolve<T>()
                : _lifetimeScope.BeginLifetimeScope().Resolve<T>();
            await action(dependency);
        }

        protected Task Run<T, U>(Func<T, U, Task> action, Action<ContainerBuilder> extraRegistration = null)
        {
            var lifetime = extraRegistration != null
                ? _lifetimeScope.BeginLifetimeScope(extraRegistration)
                : _lifetimeScope.BeginLifetimeScope();
            var dependency = lifetime.Resolve<T>();
            var dependency2 = lifetime.Resolve<U>();
            return action(dependency, dependency2);
        }

        protected async Task<TR> Run<T, TR>(Func<T, Task<TR>> action, Action<ContainerBuilder> extraRegistration = null)
        {
            var dependency = extraRegistration != null
                ? _lifetimeScope.BeginLifetimeScope(extraRegistration).Resolve<T>()
                : _lifetimeScope.BeginLifetimeScope().Resolve<T>();

            return await action(dependency);
        }
    }
}
