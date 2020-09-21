using ORM.Attribute;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ORM
{
    public class ORMHelper<T>
    {
        private SqlConnection Conn { get; set; }

        public ORMHelper(string connectString)
        {
            Conn = new SqlConnection(connectString);
        }

        public ORMHelper(SqlConnection conn)
        {
            Conn = conn;
        }


        public List<T> Query(string sql, params object[] param)
        {
            return Conn.Query<T>(sql, param);
        }

        public List<T> QueryDefault(params object[] param)
        {
            var type = typeof(T);
            if (!type.IsDefined(typeof(TableAttribute), false))
            {
                throw new Exception("Lack TableAttribute");
            }
            var tableName = GetTable();
            var columns = GetColumns();
            var sql = $@"select {(string.Join(",",columns))} from {tableName}";
            return Conn.Query<T>(sql, param);
        }

        private static List<string> GetColumns()
        {
            var columns = new List<string>();
            var type = typeof(T);
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.IsDefined(typeof(ColumnAttribute), false))
                {
                    columns.Add(prop.Name);
                }
                else
                {
                    var attribute = (prop.GetCustomAttribute(typeof(ColumnAttribute), false) as ColumnAttribute);
                    columns.Add(attribute.Get<string>());
                }
            }
            return columns;
        }

        private static string GetTable()
        {
            var type = typeof(T);
            if (!type.IsDefined(typeof(TableAttribute), false))
            {
                throw new Exception("Lack TableAttribute");
            }
            return (type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute).Get<string>();
        }
    }
}
