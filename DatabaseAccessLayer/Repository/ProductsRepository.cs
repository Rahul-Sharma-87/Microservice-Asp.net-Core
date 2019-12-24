using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DatabaseAccessLayer.Model;

namespace DatabaseAccessLayer.Repository {
    public interface IDatabaseRepository {
        //Init
        void Initialize();

        /// <summary>
        /// Add product
        /// </summary>
        long Add(object value);
    }

    public class ProductsRepository: IDatabaseRepository {

        private const string tableName = "Products";
        private const string databaseName = "ProductManagement";
        private readonly DatabaseOperation _dbOperations;

        private List<Tuple<string, SupportedDataTypes, int>> GetProductsTableSchema() {
            var schema = new List<Tuple<string, SupportedDataTypes, int>>();
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Product.Name), SupportedDataTypes.NvarChar, 20));
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Product.Category), SupportedDataTypes.NvarChar, 20));
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Product.Description), SupportedDataTypes.NvarChar, 20));
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Product.Origin), SupportedDataTypes.NvarChar, 20));
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Product.Warranty), SupportedDataTypes.integer, 0));
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Product.MRP), SupportedDataTypes.integer, 0));
            return schema;
        }

        public ProductsRepository() {
            _dbOperations = new DatabaseOperation(databaseName);
            Initialize();
        }

        public void Initialize() {
            var tablesSchema =
                new Dictionary<string, List<Tuple<string, SupportedDataTypes, int>>> {
                    {tableName, GetProductsTableSchema()}
                };
            _dbOperations.CreateDatabase(databaseName, tablesSchema);
        }

        public long Add(object value) {
            var product = value as Product;
            if (product == null) {
                throw new Exception("Product is null.");
            }
            var valueCollection = 
                new Dictionary<string, object>();
            valueCollection[nameof(Product.Name)] = product.Name;
            valueCollection[nameof(Product.Category)] = product.Category;
            valueCollection[nameof(Product.Description)] = product.Description;
            valueCollection[nameof(Product.Origin)] = product.Origin;
            valueCollection[nameof(Product.Warranty)] = product.Warranty;
            valueCollection[nameof(Product.MRP)] = product.MRP;

            return _dbOperations.InsertData(
                tableName, 
                GetProductsTableSchema(), 
                valueCollection);
        }
    }
}
