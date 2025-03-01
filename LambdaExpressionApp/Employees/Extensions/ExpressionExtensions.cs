using System.Linq.Expressions;

namespace EmployeesApi.Extensions;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> AndAlso<T>(
                        this Expression<Func<T, bool>> expr1,
                        Expression<Func<T, bool>> expr2)
    {
        // Handle null cases
        if (expr1 == null && expr2 == null)
            return null;
        if (expr1 == null)
            return expr2;
        if (expr2 == null)
            return expr1;

        // Create a new parameter to unify the expressions
        var parameter = Expression.Parameter(typeof(T));

        // Replace the original parameters in both expressions with the new parameter
        var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var leftExpr = leftVisitor.Visit(expr1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var rightExpr = rightVisitor.Visit(expr2.Body);

        // Combine the modified expressions with AndAlso
        var combinedBody = Expression.AndAlso(leftExpr, rightExpr);

        // Construct the new lambda expression
        return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
    }

    public static Expression<Func<T, bool>> OrElse<T>(
                            this Expression<Func<T, bool>> expr1,
                            Expression<Func<T, bool>> expr2)
    {
        if (expr1 == null && expr2 == null)
            return null;
        if (expr1 == null)
            return expr2;
        if (expr2 == null)
            return expr1;

        var parameter = Expression.Parameter(typeof(T));
        var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var leftExpr = leftVisitor.Visit(expr1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var rightExpr = rightVisitor.Visit(expr2.Body);

        var combinedBody = Expression.OrElse(leftExpr, rightExpr);
        return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
    }

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
