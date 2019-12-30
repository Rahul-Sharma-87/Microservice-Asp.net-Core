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
        public ActionResult<Order> Post([FromBody] Order order) {
            order.OrderId = _ordersModel.AddOrder(order);
            _rabbitManager.Publish(
                order, _brokerConfigInfo.ExchangeName, "topic",_brokerConfigInfo.RouteKey);
            return CreatedAtAction(
                nameof(GetById), 
                new { id = order.OrderId }, 
                order);
        }

        [HttpGet]
        public ActionResult<Order> GetById(int id) {
            if (id == 0) {
                return null;
            }
            var order = _ordersModel.GetOrderById(id);
            return Ok(order);
        }
    }
}
