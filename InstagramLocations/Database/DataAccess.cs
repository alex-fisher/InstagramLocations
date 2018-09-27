using System.Data;
using System.Data.SqlClient;
using InstagramLocations.Factories;
using InstagramLocations.Providers;

namespace InstagramLocations.Database
{
    public class DataAccess : IDataAccess
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IQueryFactory _queryFactory;

        private static string schema = "dbo";

        public DataAccess(IConnectionStringProvider connectionStringProvider, IQueryFactory queryFactory)
        {
            _connectionStringProvider = connectionStringProvider;
            _queryFactory = queryFactory;
        }

        public void CommitDatatable(DataTable dataTable, bool dropCreate = true)
        {
            if (dropCreate)
            {
                string dropTableStatement = $@"IF OBJECT_ID('{schema}.{dataTable.TableName}') IS NOT NULL
                                        DROP TABLE {schema}.{dataTable.TableName};";

                ExecuteNonQuery(dropTableStatement);
                ExecuteNonQuery(_queryFactory.CreateTable($"{schema}.{ dataTable.TableName}", dataTable));
            }

            BulkInsert($"{schema}.{ dataTable.TableName}", dataTable);
        }

        public void ExecuteNonQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStringProvider.GetConnectionString()))
            {
                SqlCommand command = new SqlCommand(query) {Connection = connection};

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void BulkInsert(string destinationtable, DataTable table)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStringProvider.GetConnectionString()))
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = destinationtable;
                    connection.Open();
                    bulkCopy.WriteToServer(table);
                    connection.Close();
                }
            }
        }
    }
}
