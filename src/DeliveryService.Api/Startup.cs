﻿namespace DeliveryService.Api
{
    using System;
    using System.Text.Json.Serialization;
    using System.Threading;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Autofac;
    using Dapper;
    using DeliveryService.Api.Services;
    using DeliveryService.Contracts.Messages;
    using DeliveryService.Infrastructure.Database;
    using DeliveryService.Infrastructure.Queries;
    using Microsoft.OpenApi.Models;
    using NServiceBus;
    using Serilog;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SqlMapper.AddTypeHandler(typeof(AccessWindow), new JsonTypeHandler());
            SqlMapper.AddTypeHandler(typeof(Recipient), new JsonTypeHandler());
            SqlMapper.AddTypeHandler(typeof(Order), new JsonTypeHandler());

            services.AddMvc()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllers();

            services.AddHostedService<NServiceBusService>();
            services.AddSingleton(provider =>
            {
                var nServiceBusService = provider.GetService<IHostedService>() as NServiceBusService;
                if (nServiceBusService?.MessageSession != null)
                {
                    return nServiceBusService.MessageSession;
                }

                var timeout = TimeSpan.FromSeconds(30);
                // SpinWait is here to accomodate for WebHost vs GenericHost difference
                // Closure here should be fine under the assumption we always fast track above once initialized
                if (!SpinWait.SpinUntil(() => nServiceBusService?.MessageSession != null || nServiceBusService?.StartupException != null,
                    timeout))
                {
                    throw new TimeoutException($"Unable to resolve the message session within '{timeout.ToString()}'. If you are trying to resolve the session within hosted services it is encouraged to use `Lazy<IMessageSession>` instead of `IMessageSession` directly");
                }

                nServiceBusService?.StartupException?.Throw();

                return nServiceBusService?.MessageSession;
            });
            services.AddSingleton(provider => new Lazy<IMessageSession>(provider.GetService<IMessageSession>));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Delivery Service API",
                    Version = "v1",
                    Description = "Delivery Service.",
                });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register<IDatabaseOptions>(ctx => Configuration.GetSection("Database").Get<DatabaseOptions>()).AsSelf().SingleInstance();
            builder.RegisterType<ConnectionStringProvider>().As<IConnectionStringProvider>().SingleInstance();
            builder.RegisterType<GetDeliveryByIdQuery>().As<IGetDeliveryByIdQuery>().SingleInstance();
            builder.RegisterType<GetAllDeliveriesQuery>().As<IGetAllDeliveriesQuery>().SingleInstance();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/v1/swagger.json", "Delivery Service");

                // To serve SwaggerUI at application's root page, set the RoutePrefix property to an empty string.
                c.RoutePrefix = string.Empty;
            });

            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
