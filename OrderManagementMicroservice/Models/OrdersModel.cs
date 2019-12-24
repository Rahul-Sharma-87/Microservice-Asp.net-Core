using DatabaseAccessLayer.Model;
using DatabaseAccessLayer.Repository;

namespace OrderManagementMicroservice.Models {
    public class OrdersModel {

        private IDatabaseRepository _orderRepository;

        public OrdersModel(IDatabaseRepository databaseRepository) {
            _orderRepository = databaseRepository;
        }

        public long AddOrder(Order order) {
            return _orderRepository.Add(order);
        }
    }
}
