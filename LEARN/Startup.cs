using Autofac;
using Autofac.Core;
using AutoMapper;
using LEARN.authentication;
using LEARN.extensions;
using LEARN.mappings;
using LEARN.model;
using LEARN.rabbitmq;
using MassTransit;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;


namespace LEARN
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IServiceCollection _serviceCollection;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddHttpClient();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddCustomSwagger();
            services.addcustomauthentication(Configuration);
            services.AddControllers();
            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });


            // services.AddMassTransit(x =>
            // {
            //     x.AddConsumer<CustomerASendedEventHandler>();
            //     x.AddConsumer<CustomerBSendedEventHandler>();
            //     x.UsingRabbitMq((context, cfg) =>
            //     {
            //         cfg.ReceiveEndpoint("event-listener", e =>
            //         {
            //             e.ConfigureConsumer<CustomerASendedEventHandler>(context);
            //             e.ConfigureConsumer<CustomerBSendedEventHandler>(context);
            //         });
            //     });
            // });
            //services.AddMassTransitHostedService();

            //services.AddMassTransit(x =>
            //{
            //    x.UsingRabbitMq((context, cfg) =>
            //    {
            //        cfg.ReceiveEndpoint("event-listener", e =>
            //        {
            //        });
            //    });
            //});
            //services.AddMassTransitHostedService();

            //services.AddMassTransit(x =>
            //{
            //    x.UsingRabbitMq((context, cfg) =>
            //    {
            //        cfg.Host(new Uri("rabbitmq://localhost"), h =>
            //        {
            //            h.Username("guest");
            //            h.Password("guest");
            //        });
            //    });

                services.AddMassTransit(x =>
                {
                    x.AddConsumer<createStaffConsumer>();
                    x.SetKebabCaseEndpointNameFormatter(); 
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.PrefetchCount = 1;
                        cfg.ReceiveEndpoint("event-listener", e =>
                        {
                            e.ConfigureConsumer<createStaffConsumer>(context);
                        });
                    });
                });     

           // services.AddMassTransitHostedService();

            _serviceCollection = services;
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            
            var serviceProvider = _serviceCollection.BuildServiceProvider();

          
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterModule(new appmodule(Configuration,typeof(Startup).Assembly));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "LEARN.xml"); });
            }

            app.UseRouting();
            app.UseCors("AllowAnyOrigin");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}