using Autofac;
using LEARN.authentication;
using LEARN.extensions;
using LEARN.rabbitmq;
using MassTransit;
using Hangfire;
using LEARN.hangfire;
using Hangfire.Common;

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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 启用 Hangfire 仪表板
            app.UseHangfireDashboard();
            BackgroundJob.Schedule(() => new myJob().Execute(), TimeSpan.FromMilliseconds(10));
        }
    }
}