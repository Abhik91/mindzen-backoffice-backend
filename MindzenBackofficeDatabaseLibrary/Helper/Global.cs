using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace MindzenBackofficeDatabaseLibrary.Helper
{
    public class Global
    {
        public static string? MindzenConnectionString { get; set; }
        public static string? UploadFilePath { get; set; }

        public static DataTable ConvertToDataTable<T>(List<T> items, Type type)
        {
            DataTable dataTable = new DataTable();
            PropertyInfo[] properties = type.GetProperties();

            #region Define column and its datatype
            foreach (PropertyInfo property in properties)
            {
                dataTable.Columns.Add(property.Name);
                dataTable.Columns[property.Name]!.DataType = property.PropertyType;
            }
            #endregion

            #region Add records to the Table Type
            foreach (T item in items)
            {
                DataRow dataRow = dataTable.NewRow();

                foreach (PropertyInfo property in properties)
                {
                    var column = property.Name;
                    var value = property.GetValue(item, null);
                    dataRow[column] = value;
                }

                dataTable.Rows.Add(dataRow);
            }
            #endregion

            return dataTable;
        }
    }
    
}
