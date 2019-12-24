using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace QueueManagement
{
    public static class RabbitServiceCollectionExtensions {
        public static IServiceCollection AddRabbit(
            this IServiceCollection services, 
            IConfiguration configuration
        ) {
            services.Configure<RabbitConfig>(x => configuration.GetSection("rabbit").Bind(x));
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();
            services.AddSingleton<IRabbitManager, RabbitManager>();
            return services;
        }

        public static IServiceCollection AddBrokerConfigInfo(
            this IServiceCollection services,
            IConfiguration configuration
        ) {
           services.Configure<BrokerConfigInfo>(
               option => configuration.GetSection("QueueInfo").Bind(option));
           return services;
        }
    }
}
