using DatabaseAccessLayer.Model;
using DatabaseAccessLayer.Repository;

namespace CustomerMasterMicroservice.Models {
    public class CustomerModel {

        private IDatabaseRepository _customerRepository;

        public CustomerModel(IDatabaseRepository databaseRepository) {
            _customerRepository = databaseRepository;
        }

        public void AddCustomer(Customer customer) {
            _customerRepository.Add(customer);
        }
    }
}
