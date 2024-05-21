using MassTransit;
using LEARN.Web;
using static LEARN.Web.customersended;
using Fleck;
using LEARN.Web.websocket;
using Microsoft.AspNetCore.WebSockets;
using WebSocketMiddleware = LEARN.Web.websocket.WebSocketMiddleware;

namespace LEARN.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var busControl = host.Services.GetRequiredService<IBusControl>();
            busControl.Start();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

          //  webSocket webSocket = new webSocket();


            //services.AddMassTransit(x =>
            //{
            //    x.AddConsumer<CustomerASendedEventHandler>();
            //    x.AddConsumer<CustomerBSendedEventHandler>();
            //    x.SetKebabCaseEndpointNameFormatter(); // 用于将队列名称转换为 kebab-case
            //    x.UsingRabbitMq((context, cfg) =>
            //    {
            //        cfg.ReceiveEndpoint("event-listener", e =>
            //        {
            //            e.ConfigureConsumer<CustomerASendedEventHandler>(context);
            //            e.ConfigureConsumer<CustomerBSendedEventHandler>(context);
            //        });
            //    });
            //});

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });
            });
            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            // services.AddMassTransitHostedService();

            
        } 

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

           
            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(60),
                ReceiveBufferSize = 1 * 1024
            });
            app.UseMiddleware<WebSocketMiddleware>();

           
            



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}


    //    public static void Main(string[] args)
    //    {
    //        var builder = WebApplication.CreateBuilder(args);


    //        // Add services to the container.
    //        builder.Services.AddControllersWithViews()
    //        .Services.AddMassTransit(x =>
    //        {
    //            x.AddConsumer<CustomerASendedEventHandler>();
    //            x.AddConsumer<CustomerBSendedEventHandler>();
    //            x.UsingRabbitMq((context, cfg) =>
    //            {
    //                cfg.ReceiveEndpoint("event-listener", e =>
    //                {
    //                    e.ConfigureConsumer<CustomerASendedEventHandler>(context);
    //                    e.ConfigureConsumer<CustomerBSendedEventHandler>(context);
    //                });
    //            });
    //        });

    //        builder.Services.AddMassTransitHostedService();

    //        builder.Services.AddCors(options =>
    //        {
    //            options.AddPolicy("AllowAnyOrigin",
    //                builder =>
    //                {
    //                    builder.AllowAnyOrigin()
    //                           .AllowAnyMethod()
    //                           .AllowAnyHeader();
    //                });
    //        });

    //        var app = builder.Build();

    //        // Configure the HTTP request pipeline.
    //        if (!app.Environment.IsDevelopment())
    //        {
    //            app.UseExceptionHandler("/Home/Error");
    //            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //            app.UseHsts();
    //        }

    //        app.UseHttpsRedirection();
    //        app.UseStaticFiles();

    //        app.UseRouting();

    //        app.UseAuthorization();

    //        app.MapControllerRoute(
    //            name: "default",
    //            pattern: "{controller=Home}/{action=Index}/{id?}");
    //        app.UseCors("AllowAnyOrigin");
    //        app.Run();
    //    }
    //}

