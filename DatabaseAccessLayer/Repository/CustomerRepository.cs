using System;
using System.Collections.Generic;
using DatabaseAccessLayer.Model;

namespace DatabaseAccessLayer.Repository {
    public class CustomerRepository : IDatabaseRepository {
        private const string tableName = "CustomerMaster";
        private const string databaseName = "CustomerManagement";
        private readonly DatabaseOperation _dbOperations;

        private List<Tuple<string, SupportedDataTypes, int>> GetCustomerTableSchema() {
            var schema = new List<Tuple<string, SupportedDataTypes, int>>();
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Customer.Name), SupportedDataTypes.NvarChar, 20));
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Customer.Gender), SupportedDataTypes.NvarChar, 20));
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Customer.EmailAddress), SupportedDataTypes.NvarChar, 20));
            schema.Add(
                new Tuple<string, SupportedDataTypes, int>(
                    nameof(Customer.PrimaryPhone), SupportedDataTypes.NvarChar, 20));
            return schema;
        }

        public CustomerRepository() {
            _dbOperations = new DatabaseOperation(databaseName);
            Initialize();
        }

        public void Initialize() {
            var tablesSchema =
                new Dictionary<string, List<Tuple<string, SupportedDataTypes, int>>> {
                    {tableName, GetCustomerTableSchema()}
                };
            _dbOperations.CreateDatabase(databaseName, tablesSchema);
        }

        public long Add(object value) {
            var customer = value as Customer;
            if (customer == null) {
                throw new Exception("Customer is null.");
            }
            var valueCollection = new Dictionary<string, object>();
            valueCollection[nameof(Customer.Name)] = customer.Name;
            valueCollection[nameof(Customer.Gender)] = customer.Gender.ToString();
            valueCollection[nameof(Customer.EmailAddress)] = customer.EmailAddress;
            valueCollection[nameof(Customer.PrimaryPhone)] = customer.PrimaryPhone;

            return _dbOperations.InsertData(
                tableName,
                GetCustomerTableSchema(),
                valueCollection);
        }

        public object GetObjectById(long id) {
            var valueCollection = 
                _dbOperations.GetDataById(tableName, GetCustomerTableSchema(), id);
            Customer customer = new Customer {
                CustomerId = long.Parse(valueCollection["Id"].ToString()),
                Name = valueCollection[nameof(Customer.Name)].ToString(),
                Gender = Enum.Parse<Gender>(valueCollection[nameof(Customer.Gender)].ToString()),
                EmailAddress = valueCollection[nameof(Customer.EmailAddress)].ToString(),
                PrimaryPhone = valueCollection[nameof(Customer.PrimaryPhone)].ToString()
            };
            return customer;
        }
    }
}
