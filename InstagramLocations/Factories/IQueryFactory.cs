using System.Data;

namespace InstagramLocations.Factories
{
    public interface IQueryFactory
    {
        string CreateTable(string tableName, DataTable table);
    }
}
