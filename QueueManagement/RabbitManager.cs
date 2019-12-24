using System;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace QueueManagement {
    public interface IRabbitManager {
        void Publish<T>(
            T message,
            string exchangeName,
            string exchangeType,
            string routeKey);
    }
    
    public class RabbitManager: IRabbitManager {
        private readonly DefaultObjectPool<IModel> _objectPool;
        public RabbitManager(
            IPooledObjectPolicy<IModel> policyObject,
            IOptions<BrokerConfigInfo> brokerConfigOption) {
            _objectPool = new DefaultObjectPool<IModel>(
                policyObject, Environment.ProcessorCount * 2);
        }

        public void Publish<T>(
            T message, string exchangeName, 
            string exchangeType, string routeKey) {
            if (message == null)
                return;
            //We create an object pool in the constructor. Before publishing
            //messages to RabbitMQ, we should get a channel from the object pool,
            //then construct the payload.
            var channel = _objectPool.Get();
            try {
                channel.ExchangeDeclare(exchangeName, exchangeType,true,false,null);
                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(exchangeName, routeKey, properties, sendBytes);
            } catch (Exception ex) {
                throw ex;
            } finally {
                //publishing, we should return this channel object
                //to the object pool whether the publish succeeds or fails.
                _objectPool.Return(channel);
            }
        }
    }
}
