
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
        public CustomerController(IDatabaseRepository databaseRepository ) {
            _customerModel = new CustomerModel(databaseRepository);
        }

        // POST api/values
        [HttpPost]
        public ActionResult<Customer> Post([FromBody] Customer customer) {
            var cust = _customerModel.AddCustomer(customer);
            return CreatedAtAction(nameof(GetById), new {id = cust.CustomerId}, cust);
        }

        [HttpGet]
        public ActionResult<Customer> GetById(int id) {
            if (id == 0) {
                return null;
            }
            var customer = _customerModel.GetCustomerById(id);
            return Ok(customer);
        }
    }
}
