using System;
using System.Collections.Generic;
using System.Text;
using DatabaseAccessLayer.Model;

namespace DatabaseAccessLayer.Repository {
    public class OrdersRepository : IDatabaseRepository {
        private const string tableName = "Orders";
        private const string databaseName = "OrderManagement";
        private readonly DatabaseOperation _dbOperations;

        private List<Tuple<string, SupportedDataTypes, int>> GetOrdersTableSchema() {
            var schema = new List<Tuple<string, SupportedDataTypes, int>>();
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Order.OrderDate), SupportedDataTypes.TimeStamp, 20));
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Order.ProductId), SupportedDataTypes.integer, 0));
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Order.CustomerId), SupportedDataTypes.integer, 0));
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Order.Quantity), SupportedDataTypes.integer, 0));
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Order.Discount), SupportedDataTypes.integer, 0));
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Order.TotalPrice), SupportedDataTypes.integer, 0));
            return schema;
        }

        public OrdersRepository() {
            _dbOperations = new DatabaseOperation(databaseName);
            Initialize();
        }

        public void Initialize() {
            var tablesSchema =
                new Dictionary<string, List<Tuple<string, SupportedDataTypes, int>>> {
                    {tableName, GetOrdersTableSchema()}
                };
            _dbOperations.CreateDatabase(databaseName, tablesSchema);
        }

        public long Add(object value) {
            var order = value as Order;
            if (order == null) {
                throw new Exception("Order is null.");
            }
            IDictionary<string, object> valueCollection =
                new Dictionary<string, object>();
            valueCollection[nameof(Order.ProductId)] = order.ProductId;
            valueCollection[nameof(Order.CustomerId)] = order.CustomerId;
            valueCollection[nameof(Order.Quantity)] = order.Quantity;
            valueCollection[nameof(Order.Discount)] = order.Discount;
            valueCollection[nameof(Order.TotalPrice)] = order.TotalPrice;

            return _dbOperations.InsertData(
                tableName,
                GetOrdersTableSchema(),
                valueCollection);
        }
    }
}
