using System.Linq.Expressions;

namespace Employees.Extensions;

public static partial class AppExtensions
{
    private class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldExpr;
        private readonly Expression _newExpr;

        public ReplaceExpressionVisitor(Expression oldExpr, Expression newExpr)
        {
            _oldExpr = oldExpr;
            _newExpr = newExpr;
        }

        public override Expression Visit(Expression node)
        {
            // Replace occurrences of the old parameter with the new parameter
            return node == _oldExpr ? _newExpr : base.Visit(node);
        }
    }
}
