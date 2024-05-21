using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using LEARN.data;
using LEARN.db;
using LEARN.db.unitofwork;
using LEARN.dependencyinjection;
using LEARN.Handlers;
using LEARN.mappings;
using LEARN.Middlewares;
using LEARN.redis;
using Mediator.Net;
using Mediator.Net.Autofac;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Module = Autofac.Module;

namespace LEARN
{
    public class appmodule  :   Module
    {
        private readonly IConfiguration _configuration;
        private readonly Assembly[] _assemblies;

     
        public appmodule(IConfiguration configuration,params Assembly[] assemblies)
        {
            _configuration = configuration;

            _assemblies = (assemblies ?? Array.Empty<Assembly>())
               .Concat(new[] { typeof(appmodule).Assembly })
               .Distinct()
               .ToArray();
            System.Console.WriteLine(_assemblies.Length);
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterSettings(builder);
            RegisterAutoMapper(builder);           
            RegisterDependency(builder);
            RegisterDatabase(builder);       
            RegisterMediator(builder);
            RegisterRedis(builder);

           // builder.RegisterMultiBus(_configuration, _assemblies);
        }

        private void RegisterAutoMapper(ContainerBuilder builder)
        {
            builder.RegisterAutoMapper(assemblies: _assemblies);
        }

        private static void RegisterDependency(ContainerBuilder builder)
        {
            foreach (var type in typeof(IDependency).Assembly.GetTypes()
                         .Where(type => type.IsClass && typeof(IDependency).IsAssignableFrom(type)))
                if (typeof(IScopedDependency).IsAssignableFrom(type))
                    builder.RegisterType(type).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
                else if (typeof(ISingletonDependency).IsAssignableFrom(type))
                    builder.RegisterType(type).AsSelf().AsImplementedInterfaces().SingleInstance();
                else if (typeof(ITransientDependency).IsAssignableFrom(type))
                    builder.RegisterType(type).AsSelf().AsImplementedInterfaces().InstancePerDependency();
                else
                    builder.RegisterType(type).AsSelf().AsImplementedInterfaces();
        }

        private void RegisterMediator(ContainerBuilder builder)
        {
            var mediatorBuilder = new MediatorBuilder();


            mediatorBuilder.RegisterHandlers(_assemblies);
            mediatorBuilder.ConfigureGlobalReceivePipe(c =>
            {
                //c.UseLogger();
                //c.UseMessageValidator();
                c.UseUnitOfWork();
            });
            builder.RegisterMediator(mediatorBuilder);
        }

        private void RegisterRedis(ContainerBuilder builder)
        { 
           // string redisConnection = _configuration.GetValue<string>("redis");
            builder.RegisterType<redishelp>()
               // .WithParameter("redisconnection", redisConnection)
                 .AsSelf()
                    .SingleInstance();

            builder.RegisterType<redisop>()
                 .AsSelf()
                 .SingleInstance();

            var serviceCollection = new ServiceCollection();
            serviceCollection.Configure<redisoption>(action =>
            {
                action.redisconnection = _configuration.GetValue<string>("redis");
            });

            builder.Populate(serviceCollection);
        }


        private void RegisterSettings(ContainerBuilder builder)
        {
            builder.RegisterInstance(_configuration)
                .As<IConfiguration>()
                .SingleInstance();

            var serviceCollection = new ServiceCollection();

            serviceCollection.Configure<mysqloption>(action =>
            {
                action.ConnectionString = _configuration.GetConnectionString("Mysql");
                action.Version = _configuration.GetValue<string>("ConnectionStrings:Version");
            });
        
            // var settingTypes = typeof(ApplicationModule).Assembly.GetTypes()
            //     .Where(t => t.IsClass && typeof(IConfigurationSetting).IsAssignableFrom(t))
            //     .ToArray();

            builder.Populate(serviceCollection);
        }

        private void RegisterDatabase(ContainerBuilder builder)
        {
            builder.RegisterType<appdbcontext>()
                .AsSelf()
                .As<DbContext>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(repository<>))
                .As(typeof(irepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<unitofwork>().As<iunitofwork>().InstancePerLifetimeScope();
           // builder.RegisterType<FileSystemDbUpScriptRunner>().AsImplementedInterfaces();
        }
    }
}
