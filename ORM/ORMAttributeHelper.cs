using ORM.Attribute;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ORM
{
    public class ORMAttributeHelper
    {
        public static string GetPropField(PropertyInfo prop)
        {
            if (prop.IsDefined(typeof(ColumnAttribute), false))
            {
                return (prop.GetCustomAttribute(typeof(ColumnAttribute), false) as ColumnAttribute).Get<string>();
            }
            return prop.Name;
        }
    }

    public class ORMCacheHelper<T>
    {
        private static List<string> Columns { get; set; }
        private static string TableName { get; set; }

        public static string GetTable()
        {
            if (TableName != null) return TableName;
            var type = typeof(T);
            if (!type.IsDefined(typeof(TableAttribute), false))
            {
                throw new Exception("Lack TableAttribute");
            }
            var name = (type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute).Get<string>();
            TableName = name;
            return name;
        }

        public static List<string> GetColumns()
        {
            if (Columns != null) return Columns;
            var columns = new List<string>();
            var type = typeof(T);
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                columns.Add(ORMAttributeHelper.GetPropField(prop));
            }
            Columns = columns;
            return columns;
        }

    }
}
