using Microsoft.AspNetCore.Mvc;
using DatabaseAccessLayer.Repository;
using CustomerMasterMicroservice.Models;
using DatabaseAccessLayer.Model;

namespace CustomerMasterMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase {

        private CustomerModel _customerModel;
        public CustomerController(IDatabaseRepository databaseRepository) {
            _customerModel = new CustomerModel(databaseRepository);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Customer customer) {
            _customerModel.AddCustomer(customer);
        }
    }
}
