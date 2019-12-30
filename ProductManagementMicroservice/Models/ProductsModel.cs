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

        public long AddProduct(Product product) {
            return _productRepository.Add(product);
        }

        internal Product GetProductById(int id) {
            return _productRepository.GetObjectById(id) as Product;
        }
    }
}
