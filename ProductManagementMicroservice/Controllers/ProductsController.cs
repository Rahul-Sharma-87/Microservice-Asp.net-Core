using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountsMicroservice.Models;
using DatabaseAccessLayer.Model;
using DatabaseAccessLayer.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AccountsMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase {

        private ProductsModel _productModel;

        public ProductsController(IDatabaseRepository repository) {
            _productModel = new ProductsModel(repository);
        }

        [HttpPost]
        public void Post([FromBody] Product value) {
            _productModel.AddProduct(value);
        }
    }
}
