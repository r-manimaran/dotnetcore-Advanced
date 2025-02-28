using Employees.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Employees.Extensions;

public static partial class AppExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            await using (var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
            {
                await dbContext.Database.MigrateAsync();
                await DatabaseSeedingService.SeedAsync(dbContext);
            }

        }
    }

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
}
