using System;
using System.Collections.Generic;
using System.Text;

namespace ORM.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnAttribute : ORMAttribute
    {
        public ColumnAttribute(string value) : base(value)
        {

        }
    }
}
