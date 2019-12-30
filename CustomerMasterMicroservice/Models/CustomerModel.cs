using DatabaseAccessLayer.Model;
using DatabaseAccessLayer.Repository;

namespace CustomerMasterMicroservice.Models {
    public class CustomerModel {

        private IDatabaseRepository _customerRepository;

        public CustomerModel(IDatabaseRepository databaseRepository) {
            _customerRepository = databaseRepository;
        }

        public Customer AddCustomer(Customer customer) {
            customer.CustomerId = _customerRepository.Add(customer);
            return customer;
        }

        public Customer GetCustomerById(long id) {
            return _customerRepository.GetObjectById(id) as Customer;
        }
    }
}
