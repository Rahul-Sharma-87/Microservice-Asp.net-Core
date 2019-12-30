using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace DatabaseAccessLayer {

    public enum SupportedDataTypes {
        NvarChar,
        integer,
        Long,
        DateTime,
        TimeStamp
    }

    public class DatabaseOperation {
        private SQLiteConnection _dbConnection;
        public DatabaseOperation(string dbName) {
            _dbConnection = new SQLiteConnection($"Data Source={dbName}.sqlite;Version=3;");
        }

        public void CreateDatabase(
            string name, 
            Dictionary<string, List<Tuple<string, SupportedDataTypes, int>>> tableSchemas
        ) {
            try {
                _dbConnection.Open();
                foreach (var schemas in tableSchemas) {
                    if (!CheckTableExists(schemas.Key)) {
                        string tableSql = CreateTable(schemas.Key, schemas.Value);
                        SQLiteCommand command = 
                            new SQLiteCommand(tableSql, _dbConnection);
                        command.ExecuteNonQuery();
                    }
                }
            } finally{
                _dbConnection.Close();
            }
        }

        private string CreateTable(
            string tableName, 
            List<Tuple<string, SupportedDataTypes, int>> tableSchema
        ) {
            StringBuilder stringBuilder = 
                new StringBuilder("Create table ");
            stringBuilder.Append(tableName);
            stringBuilder.Append(" ( ");
            stringBuilder.Append(" Id  INTEGER PRIMARY KEY, ");
            var columns =
                tableSchema.Select(
                    schema => {
                        if (schema.Item2.Equals(SupportedDataTypes.integer)) {
                            return $" {schema.Item1} INTEGER ";
                        } else if (schema.Item2.Equals(SupportedDataTypes.TimeStamp)) {
                            return $" {schema.Item1} DATETIME DEFAULT CURRENT_TIMESTAMP ";
                        } else if (schema.Item2.Equals(SupportedDataTypes.TimeStamp)) {
                            return $" {schema.Item1} DATETIME DEFAULT CURRENT_TIMESTAMP ";
                        } else {
                            return $" {schema.Item1} VARCHAR ({schema.Item3}) ";
                        }
                    }
                );
            var columnsCs = string.Join(',', columns);
            stringBuilder.Append(columnsCs);
            stringBuilder.Append(" ) ");
            return stringBuilder.ToString();
        }

        public long InsertData(
            string tableName, 
            List<Tuple<string, SupportedDataTypes, int>> tableSchemas,
            IDictionary<string,object> columnValues
        ) {
            long pkId;
            //if (tableSchemas.Count != columnValues.Count) {
            //    throw new Exception();
            //}
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO ");
            sqlBuilder.Append(tableName);
            sqlBuilder.Append(" ( ");

            var items = columnValues.Keys;
            //var items = tableSchemas.Select(x => x.Item1);
            var columnsConcat = string.Join(',', items);
            sqlBuilder.Append(columnsConcat);
            sqlBuilder.Append(" ) VALUES (");
            var itemParams = items.Select(x => $"@{x}");
            var columnsParamConcat = string.Join(',', itemParams);
            sqlBuilder.Append(columnsParamConcat);
            sqlBuilder.Append(" ) ");

            try {
                _dbConnection.Open();
                using (SQLiteCommand command =
                    new SQLiteCommand(sqlBuilder.ToString(), _dbConnection)) {
                    command.CommandType = CommandType.Text;
                    foreach (var column in columnValues) {
                        var item = tableSchemas.FirstOrDefault(
                            x => x != null &&
                            x.Item1.Equals(
                                column.Key,
                                StringComparison.InvariantCultureIgnoreCase
                            ));
                        if (item != null) {
                            var dbType = DbType.String;
                            if (item.Item2.Equals(SupportedDataTypes.integer)) {
                                dbType = DbType.Int32;
                            } else if(item.Item2.Equals(SupportedDataTypes.Long)) {
                                dbType = DbType.Int64;
                            } else if (item.Item2.Equals(SupportedDataTypes.DateTime)) {
                                dbType = DbType.DateTime;
                            }
                            var param = new SQLiteParameter($"@{column.Key}", dbType);
                            param.Value = column.Value;
                            command.Parameters.Add(param);
                        }
                    }
                    command.ExecuteScalar();
                    pkId = _dbConnection.LastInsertRowId;
                }
            } finally {
                _dbConnection.Close();
            }
            return pkId;
        }

        public bool CheckTableExists(string tableName) {
            string sqlCmd = $"SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = '{tableName}'";
            using (SQLiteCommand command =
                new SQLiteCommand(sqlCmd, _dbConnection)
            ) {
                command.CommandType = CommandType.Text;
                var output = command.ExecuteScalar();
                if (
                    output != null &&
                    int.TryParse(output.ToString(), out int exists)
                ) {
                    return exists > 0;
                }
            }
            return false;
        }

        public void UpdateData()
        {

        }

        public IDictionary<string, object> GetDataById(
            string tableName, 
            List<Tuple<string, SupportedDataTypes, int>> tableSchemas,
            long Id
        ) {
            IDictionary<string, object> valueCollection = 
                new Dictionary<string, object>();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine($"SELECT * FROM {tableName} WHERE Id={Id}");
            try {
                _dbConnection.Open();
                using (SQLiteCommand command =
                    new SQLiteCommand(sqlBuilder.ToString(), _dbConnection)) {
                    command.CommandType = CommandType.Text;
                    var reader = command.ExecuteReader();
                    while (reader.Read()) {
                        valueCollection["Id"] = reader["Id"];
                        foreach (var tableSchema in tableSchemas) {
                            valueCollection[tableSchema.Item1] = reader[tableSchema.Item1];
                        }
                    }
                }
            } finally {
                _dbConnection.Close();
            }
            return valueCollection;
        }
    }
}
