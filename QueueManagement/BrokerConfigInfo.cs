
namespace QueueManagement {
    public class BrokerConfigInfo {
        public string ExchangeName  { get; set; }

        public string QueueName { get; set; }

        public string RouteKey { get; set; }

        public string ExchangeType { get; set; }
    }
    //"QueueInfo": {
    //"ExchangeName": "Order.invoice.exchange",
    //"QueueName": "Order.invoice.exchange.queue",
    //"RouteKey": "Order.invoice.exchange.queue.neworder",
    //"ExchangeType": "/"
}
