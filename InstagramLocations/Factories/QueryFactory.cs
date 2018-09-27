using System.Collections.Generic;
using System.Data;

namespace InstagramLocations.Factories
{
    public class QueryFactory : IQueryFactory
    {
        private static readonly Dictionary<string, string> DataTypeMapping = new Dictionary<string, string> {
                                                                                            {"String",    "VARCHAR(1024)"},
                                                                                            {"Decimal",   "DECIMAL(19,2)"},
                                                                                            {"Double",    "DECIMAL(19,2)"},
                                                                                            {"Int16",     "INT"},
                                                                                            {"Int32",     "INT"},
                                                                                            {"Int64",     "INT"},
                                                                                            {"Byte[]",      "VARBINARY(MAX)"},
                                                                                            {"DateTime",  "DATETIME"},
                                                                                            {"Guid",      "UNIQUEIDENTIFIER"},
                                                                                            };
        private const string Createtable = "CREATE TABLE {0} ({1})";

        public string CreateTable(string tableName, DataTable table)
        {
            string columnData = "";
            foreach (DataColumn column in table.Columns)
            {
                columnData += GetColumnString(column);
            }

            return string.Format(Createtable, tableName, columnData);
        }

        private string GetColumnString(DataColumn column)
        {
            string name = GetColumnName(column.ColumnName);
            string dataType = GetDataType(column.DataType.Name);
            string nullable = GetNullable(column.AllowDBNull);

            return string.Format("{0} {1} {2},\n", name, dataType, nullable);
        }

        private string GetColumnName(string name)
        {
            return string.Format("[{0}]", name);
        }

        private static string GetDataType(string dataType)
        {
            return DataTypeMapping[dataType];
        }

        private string GetNullable(bool nullable)
        {
            if (nullable)
            {
                return "NULL";
            }
            else
                return "NOT NULL";
        }

        public static string Select(string tableName, string dataBase = "Scratchpad", string schema = "dbo")
        {
            return $"Select * from {dataBase}.{schema}.{tableName}{System.Environment.NewLine}";
        }
    }
}