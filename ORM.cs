using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ORM
{
    public static class ORM
    {
        private static object GetClassProperty(IDataReader reader,Type type)
        {
            var instance = Activator.CreateInstance(type);
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var field = reader.GetName(i);
                var prop = type.GetProperty(field);
                if (prop.PropertyType.IsPrimitive)
                {
                    prop.SetValue(instance, reader[field]);
                }
                else
                {
                    prop.SetValue(instance, GetClassProperty(reader,prop.PropertyType));
                }

            }
            return instance;
        }

        private static IDbCommand CreateCommand(IDbConnection cnn, string sql,object param)
        {
            var cmd = cnn.CreateCommand();
            cmd.CommandText = sql;
            if (param is IEnumerable o)
            {
                foreach (var item in o)
                {
                    cmd.Parameters.Add(item);
                }
            }
            else
            {
                cmd.Parameters.Add(param);
            }
            return cmd;
        }

        public static List<T> Query<T>(this IDbConnection cnn, string sql, object param = null)
        {
            var dataList = new List<T>();
            var cmd = CreateCommand(cnn,sql, param);
            var type = typeof(T);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dataList.Add((T)GetClassProperty(reader, type));
            }
            reader.Close();
            return dataList;
        }

        public static List<object> Query(this IDbConnection cnn, string sql, object param = null)
        {
            var dataList = new List<dynamic>();
            var cmd = CreateCommand(cnn, sql, param);
            var type = typeof(object);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dataList.Add(GetClassProperty(reader, type));
            }
            reader.Close();
            return dataList;
        }

        public static List<T> QueryFirst<T>(this IDbConnection cnn, string sql, object param = null)
        {
            var dataList = new List<T>();
            var cmd = CreateCommand(cnn, sql, param);
            var type = typeof(T);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dataList.Add((T)GetClassProperty(reader, type));
                break;
            }
            reader.Close();
            return dataList;
        }

        public static int Execute(this IDbConnection cnn, string sql, object param = null)
        {
            var cmd = CreateCommand(cnn, sql, param);
            return cmd.ExecuteNonQuery();
        }
    }
}
