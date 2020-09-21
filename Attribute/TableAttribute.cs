using System;
using System.Collections.Generic;
using System.Text;

namespace ORM.Attribute
{

    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public class TableAttribute: ORMAttribute
    {
        public TableAttribute(string value) : base(value)
        {

        }
    }
}
