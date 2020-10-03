using ORM.Attribute;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ORM
{
    public static class ORM
    {
        private static object GetClassProperty(IDataReader reader, Type type)
        {
            var instance = Activator.CreateInstance(type);
            var fieldDic = new Dictionary<string, string>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var field = reader.GetName(i);
                fieldDic.Add(field, field);
            }

            foreach (var prop in type.GetProperties())
            {
                var field = ORMAttributeHelper.GetPropField(prop);
                if (!fieldDic.ContainsKey(field)) continue;
                if (prop.PropertyType.IsPrimitive || prop.PropertyType.Equals(typeof(string)))
                {
                    prop.SetValue(instance, reader[fieldDic[field]]);
                }
                else
                {
                    prop.SetValue(instance, GetClassProperty(reader, prop.PropertyType));
                }
            }
            return instance;
        }

        private static dynamic GetDynamicProperty(IDataReader reader)
        {
            var instance = new ExpandoObject();
            var dic = (IDictionary<string, object>)instance;
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var field = reader.GetName(i);
                dic[field] = reader[field];
            }
            return instance;
        }

        private static void AddParameter(IDbCommand cmd, object item)
        {
            var type = item.GetType();
            foreach (var prop in type.GetProperties())
            {
                var v = prop.GetValue(item);
                if (v is ICollection)
                {
                    HandleInClause(cmd, prop.Name, v as ICollection);
                }
                else
                {
                    var param = new SqlParameter($"@{prop.Name}", prop.GetValue(item));
                    cmd.Parameters.Add(param);
                }
            }
        }

        private static void HandleInClause(IDbCommand cmd, string field, ICollection obj)
        {
            var replces = new List<string>();
            var count = 0;
            foreach (var item in obj)
            {
                var newField = $@"@{field}{count}";
                replces.Add(newField);
                var param = new SqlParameter(newField, item);
                cmd.Parameters.Add(param);
                count++;
            }
            cmd.CommandText = cmd.CommandText.Replace($@"@{field}", $@"({string.Join(",", replces)})");
        }

        private static IDbCommand CreateCommand(IDbConnection cnn, string sql, object param)
        {
            var cmd = cnn.CreateCommand();
            cmd.CommandText = sql;

            if (param is IEnumerable o)
            {
                foreach (var item in o)
                {
                    AddParameter(cmd, item);
                }
            }
            else
            {
                AddParameter(cmd, param);
            }
            return cmd;
        }

        public static List<T> Query<T>(this IDbConnection cnn, string sql, object param = null)
        {
            var dataList = new List<T>();
            var cmd = CreateCommand(cnn, sql, param);
            var type = typeof(T);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dataList.Add((T)GetClassProperty(reader, type));
            }
            reader.Close();
            return dataList;
        }

        public static List<dynamic> Query(this IDbConnection cnn, string sql, object param = null)
        {
            var dataList = new List<dynamic>();
            var cmd = CreateCommand(cnn, sql, param);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dataList.Add(GetDynamicProperty(reader));
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
