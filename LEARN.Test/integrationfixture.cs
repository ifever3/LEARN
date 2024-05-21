using System.IO;
using System.Threading.Tasks;
using Autofac;
using LEARN.db;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MySqlConnector;
using NSubstitute;
using Pomelo.EntityFrameworkCore.MySql.Internal;
using Respawn;
using Xunit;

namespace LEARN.Test
{
    public class integrationfixture : IAsyncLifetime
    {
        public readonly ILifetimeScope LifetimeScope;
        private readonly mysqloption _mySqlOptions;
        private Respawner _respawner;

        public integrationfixture()
        {
            var containerBuilder = new ContainerBuilder();
          //  var logger = Substitute.For<ILogger>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings1.json", false, true).Build();

            containerBuilder.RegisterModule(new appmodule( configuration,
                typeof(integrationfixture).Assembly));

          //  RegisterIdentity(containerBuilder);

            LifetimeScope = containerBuilder.Build();
            _mySqlOptions = LifetimeScope.Resolve<IOptions<mysqloption>>().Value;
        }
        public async Task InitializeAsync()
        {
            await using var conn = new MySqlConnection(_mySqlOptions.ConnectionString);
            await conn.OpenAsync();

            _respawner = await Respawner.CreateAsync(conn,
                new RespawnerOptions
                {
                    SchemasToInclude = ["learntest"],
                    TablesToIgnore =
                [
                    "staff"
                ],
                    DbAdapter = DbAdapter.MySql
                });
        }
        public Task DisposeAsync()
        {
            var context = LifetimeScope.Resolve<appdbcontext>();
            return context.Database.EnsureDeletedAsync();
        }

        public async Task ResetDatabase()
        {
            await using var conn = new MySqlConnection(_mySqlOptions.ConnectionString);
            await conn.OpenAsync();
            await _respawner.ResetAsync(conn);
        }    
    }
}
