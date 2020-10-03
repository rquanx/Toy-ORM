using ORM.Attribute;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ORM
{
    public class ORMHelper
    {
        private SqlConnection Conn { get; set; }

        public ORMHelper()
        {
            Conn = new SqlConnection(
                "Data Source = localhost;Initial Catalog = TestDB;User Id = sa;Password = 123456;");
            Conn.Open();
        }

        public ORMHelper(string connectString)
        {
            Conn = new SqlConnection(connectString); Conn.Open();
        }

        public ORMHelper(SqlConnection conn)
        {
            Conn = conn; Conn.Open();
        }

        public dynamic Query(string sql, params object[] param)
        {
            return Conn.Query(sql, param);
        }

        public List<T> Query<T>(string sql, params object[] param)
        {
            return Conn.Query<T>(sql, param);
        }

        public List<T> QueryFirst<T>(string sql, params object[] param)
        {
            return Conn.QueryFirst<T>(sql, param);
        }

        public List<T> Query<T>(Expression exp, params object[] param)
        {
            var sql = "";
            return Conn.Query<T>(sql, param);
        }

        public bool Delete<T>(Expression<Func<T,bool>> exp, params object[] param)
        {
            var sql = "";
            return true;
        }

        public List<T> QueryDefault<T>(params object[] param)
        {
            var type = typeof(T);
            var tableName = ORMCacheHelper<T>.GetTable();
            var columns = ORMCacheHelper<T>.GetColumns();
            var sql = $@"select {(string.Join(",",columns))} from {tableName}";
            return Conn.Query<T>(sql, param);
        }
    }
}
