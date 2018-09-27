using System;
using System.Collections.Generic;
using System.Data;

namespace InstagramLocations.Extensions
{
    public static class ObjectExtensions
    {
        public static DataTable ToDatatable<T>(this IList<T> objectList)
        {
            DataTable table = new DataTable();

            foreach (var field in typeof(T).GetFields())
            {
                Type fieldType = field.FieldType;

                if (fieldType.IsEnum)
                    fieldType = typeof(string);

                table.Columns.Add(field.Name, fieldType);
            }

            foreach (var obj in objectList)
            {
                DataRow dataRow = table.NewRow();
                foreach (var field in typeof(T).GetFields())
                {
                    var fieldValue = field.GetValue(obj);

                    dataRow[field.Name] = fieldValue;
                }

                table.Rows.Add(dataRow);
            }

            return table;
        }
    }
}
