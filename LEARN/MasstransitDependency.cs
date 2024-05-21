using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;

namespace LEARN
{
    public static class MasstransitDependency
    {
        public static void RegisterMultiBus(this ContainerBuilder builder, IConfiguration configuration,
            params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
                throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan.");

            var services = new ServiceCollection();
          /*  var foundationMqSetting = new FoundationRabbitMqSetting(configuration);
            var scanTypes = assemblies.SelectMany(a => a.GetTypes());

            if (foundationMqSetting.IsEnabled)
            {
                services.AddMassTransit<IFoundationBus>(x =>
                {
                    var foundationEventConsumers = scanTypes.Where(type =>
                            type.IsClass && IsAssignableToGenericType(type, typeof(IFoundationEventConsumer<>)))
                        .ToList();

                    foreach (var type in foundationEventConsumers) x.AddConsumer(type);

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(foundationMqSetting.Host, h =>
                        {
                            h.Username(foundationMqSetting.UserName);
                            h.Password(foundationMqSetting.Password);
                        });

                        cfg.ReceiveEndpoint(foundationMqSetting.QueueName, e =>
                        {
                            foreach (var foundationEventConsumer in foundationEventConsumers)
                                e.ConfigureConsumer(context, foundationEventConsumer);
                        });
                    });
                });
            }*/

            if (services.Any())
                builder.Populate(services);
        }

        private static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType)) return true;

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType) return true;

            var baseType = givenType.BaseType;
            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }
    }
}
