using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ORM
{
    public class ExpressionHelper: ExpressionVisitor
    {

        public override Expression Visit(Expression exp)
        {

            return base.Visit(exp);
        }
    }
}
