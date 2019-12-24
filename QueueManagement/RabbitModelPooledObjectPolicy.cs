using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace QueueManagement {
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly RabbitConfig _options;

        private readonly IConnection _connection;

        public RabbitModelPooledObjectPolicy(
            IOptions<RabbitConfig> optionsAcc
        ){
            _options = optionsAcc.Value;
            _connection = GetConnection();
        }

        private IConnection GetConnection() {
            var factory = new ConnectionFactory() {
                HostName = _options.HostName,
                VirtualHost = _options.VHost,
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port
            };
            return factory.CreateConnection();
        }

        // Create method tells the pool how to create the channel object.
        public IModel Create() {
            return _connection.CreateModel();
        }
        /// <summary>
        /// The Return method tells the pool that if the channel object
        /// is still in a state that can be used, we should return it to the pool;
        /// otherwise, we should not use it the next time.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Return(IModel obj) {
            if (obj.IsOpen) {
                return true;
            } 
            obj?.Dispose();
            return false;
        }
    }
}
