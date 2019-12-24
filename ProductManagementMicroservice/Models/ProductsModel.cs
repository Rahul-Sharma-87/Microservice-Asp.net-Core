using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseAccessLayer.Model;
using DatabaseAccessLayer.Repository;

namespace AccountsMicroservice.Models {
    public class ProductsModel {

        private IDatabaseRepository _productRepository;

        public ProductsModel(IDatabaseRepository databaseRepository) {
            _productRepository = databaseRepository;
        }

        public void AddProduct(Product product) {
            _productRepository.Add(product);
        }

        public void UpdateProduct(Product product)
        {

        }

        public void DeleteProduct() {

        }
    }
}
