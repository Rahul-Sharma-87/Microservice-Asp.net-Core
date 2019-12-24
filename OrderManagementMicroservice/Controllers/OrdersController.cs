using Microsoft.AspNetCore.Mvc;
using DatabaseAccessLayer.Model;
using OrderManagementMicroservice.Models;
using DatabaseAccessLayer.Repository;
using Microsoft.Extensions.Options;
using QueueManagement;

namespace OrderManagementMicroservice.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase {

        private readonly IRabbitManager _rabbitManager;

        private readonly OrdersModel _ordersModel;

        private readonly BrokerConfigInfo _brokerConfigInfo;

        public OrdersController(
            IDatabaseRepository databaseRepository, 
            IRabbitManager rabbitManager,
            IOptions<BrokerConfigInfo> brokerOptions
        ) {
            _ordersModel = new OrdersModel(databaseRepository);
            _rabbitManager = rabbitManager;
            _brokerConfigInfo = brokerOptions.Value;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Order value) {
            value.OrderId = _ordersModel.AddOrder(value);
            _rabbitManager.Publish(
                value, _brokerConfigInfo.ExchangeName, "topic",_brokerConfigInfo.RouteKey);
        }

    }
}
