using System.Data;

namespace InstagramLocations.Database
{
    public interface IDataAccess
    {
        void CommitDatatable(DataTable dataTable, bool dropCreate = true);
        void ExecuteNonQuery(string query);
        void BulkInsert(string destinationtable, DataTable table);
    }
}
