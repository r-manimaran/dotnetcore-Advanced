# EF Core Advanced Features

This repository demonstrates advanced features and performance optimization techniques in Entity Framework Core.

## Features Covered

### Compiled Queries
Demonstrates the implementation and performance comparison of normal vs compiled queries in EF Core.

#### Normal Query vs Compiled Query
```csharp
// Normal Query
public async Task<List<Product>> GetProductsNormalQuery()
{
    return await _dbContext.Products
                          .AsNoTracking()
                          .ToListAsync();
}

// Compiled Query
private static readonly Func<AppDbContext, IAsyncEnumerable<Product>> _compiledQuery = 
    EF.CompileAsyncQuery((AppDbContext context) => 
        context.Products.AsNoTracking());
```

### Key Insights
 1. Query Translation Process

        - Normal queries: Parse LINQ → Translate to SQL →  Generate plan → Execute

        - Compiled queries: Use cached plan → Execute

2. When to Use Compiled Queries

    - Complex queries with multiple conditions

    - High-frequency query execution

    - Performance-critical scenarios

    - Large datasets

3. Benefits

    - Eliminates expression tree parsing overhead

    - Eliminates LINQ to SQL translation overhead

    - Reuses cached execution plans

4. Trade-offs

    - Higher memory usage (stored compiled plans)

    - Less query flexibility

    - May not benefit simple queries significantly

### Best Practices

    - Use compiled queries for frequently executed complex queries
    - Consider memory trade-offs
    - Benchmark your specific use case
    - Use AsNoTracking() for read-only scenarios