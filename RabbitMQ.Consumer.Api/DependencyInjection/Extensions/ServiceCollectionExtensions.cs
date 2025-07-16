using MassTransit;
using RabbitMQ.Consumer.Api.DependencyInjection.Options;
using System.Reflection;

namespace RabbitMQ.Producer.Api.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediat(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
        public static IServiceCollection AddConfigureMasstransitRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var masstransitConfiguration = new MasstransitConfiguration();
            configuration.GetSection(nameof(MasstransitConfiguration)).Bind(masstransitConfiguration);

            services.AddMassTransit(mt =>
            {
               // mt.AddConsumer<SendSmsWhenReceivedSmsEventConsumer>();
                mt.AddConsumers(Assembly.GetExecutingAssembly()); //add consumers from the current assembly
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(
                        masstransitConfiguration.Host,
                        masstransitConfiguration.VHost,
                        h =>
                        {
                            h.Username(masstransitConfiguration.UserName);
                            h.Password(masstransitConfiguration.Password);
                        });

                    cfg.ConfigureEndpoints(context);
                });
            });
            return services;
        }
    }
}
