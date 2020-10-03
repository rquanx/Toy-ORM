using ORM;
using ORM.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace ORMTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var orm = new ORMHelper();
            var r1 = orm.Query<TestModel>("select * from Test where Name = @Name and Content = @Content", new {
                Content = "cc"
            },new { 
                Name = "bb"
            });
            var r2 = orm.QueryDefault<TestModel>();
            var r3 = orm.Query("select * from Test where Name = @Name and Content = @Content", new
            {
                Content = "cc"
            }, new
            {
                Name = "bb"
            });
            var a = r3[0];
            var r4 = orm.QueryFirst<TestModel>("select * from Test");

            var r5 = orm.Query<TestModel>("select * from Test where ID in @ID",new { ID = new List<int>() { 1,3} });
        }
    }
}
