using System;
using System.Collections.Generic;
using System.Text;

namespace ORM.Attribute
{
    public abstract class ORMAttribute: System.Attribute
    {
        private object Value { get; set; }

        public ORMAttribute(object value)
        {
            Value = value;
        }

        public T Get<T>()
        {
            return (T)Value;
        }
    }
}
