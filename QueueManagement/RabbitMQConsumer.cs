using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QueueManagement {
    
    public class RabbitMQConsumer:BackgroundService {
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;
        private BrokerConfigInfo _brokerConfigInfo;

        public RabbitMQConsumer(
            IOptions<BrokerConfigInfo> configOption,
            IBackgroundTaskQueue taskQueue,
            ILoggerFactory loggerFactory)
        {
            _brokerConfigInfo = configOption.Value;
            _logger = loggerFactory.CreateLogger<RabbitMQConsumer>();
            TaskQueue = taskQueue;
            InitQueue();
        }

        public IBackgroundTaskQueue TaskQueue { get; }

        private void InitQueue() {
            var factory = new ConnectionFactory() {HostName = "localhost"};
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(
                _brokerConfigInfo.ExchangeName, 
                ExchangeType.Topic, 
                true, false, null);
            _channel.QueueDeclare(_brokerConfigInfo.QueueName, false, false, false, null);
            _channel.QueueBind(
                _brokerConfigInfo.QueueName, 
                _brokerConfigInfo.ExchangeName, 
                _brokerConfigInfo.RouteKey, 
                null);
            _channel.BasicQos(0, 1, false);
            _connection.ConnectionShutdown += _connection_ConnectionShutdown;
            _logger.LogInformation("Consumer successfully instantiated");
        }

        private void _connection_ConnectionShutdown(object sender, ShutdownEventArgs e){
            //ignore
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, e) => {
                var content = System.Text.Encoding.UTF8.GetString(e.Body);
                _logger.LogInformation(content);
                await BackgroundProcessing(stoppingToken);
                _channel.BasicAck(e.DeliveryTag, false);
            };
            consumer.Shutdown += Consumer_Shutdown;
            consumer.Registered += Consumer_Registered;
            consumer.Unregistered += Consumer_Unregistered;
            consumer.ConsumerCancelled += Consumer_ConsumerCancelled;
            _channel.BasicConsume(_brokerConfigInfo.QueueName, false, consumer);
            return Task.CompletedTask;
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                var workItem =
                    await TaskQueue.DequeueAsync(stoppingToken);
                if (workItem == null) {
                    TaskQueue.InitializeQueue();
                    return;
                }
                try {
                    await workItem(stoppingToken);
                } catch (Exception ex) {
                    _logger.LogError(ex,
                        "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }
        }

        private void Consumer_ConsumerCancelled(object sender, ConsumerEventArgs e) {
            //ignore
        }

        private void Consumer_Unregistered(object sender, ConsumerEventArgs e) {
            //ignore
        }

        private void Consumer_Registered(object sender, ConsumerEventArgs e) {
            //ignore
        }

        private void Consumer_Shutdown(object sender, ShutdownEventArgs e) {
            //ignore
        }
        public override void Dispose() {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
