
using AccountsMicroservice.Models;
using DatabaseAccessLayer.Model;
using DatabaseAccessLayer.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AccountsMicroservice.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase {

        private ProductsModel _productModel;

        public ProductsController(IDatabaseRepository repository) {
            _productModel = new ProductsModel(repository);
        }

        [HttpPost]
        public ActionResult<Product> Post([FromBody] Product product) {
            product.ProductId =_productModel.AddProduct(product);
            return CreatedAtAction(
                nameof(GetById), 
                new { id = product.ProductId }, 
                product);
        }

        [HttpGet]
        public ActionResult<Product> GetById(int id) {
            if (id == 0) {
                return null;
            }
            var product = _productModel.GetProductById(id);
            return Ok(product);
        }
    }
}
