using ConnectionPoolApi.Services;
using System.Diagnostics;

namespace ConnectionPoolApi;

public class QueryBenchmark
{
    private readonly IProductService _productService;
    private readonly ILogger<QueryBenchmark> _logger;

    public QueryBenchmark(IProductService productService, ILogger<QueryBenchmark> logger)
    {
        _productService = productService;
        _logger = logger;
    }
    
    public async Task Benchmark()
    {
        // Warm up (first execution includes compilation overhead for both)
        // await _productService.GetAllProductsAsync();
        // await _productService.GetAllProductsCompiledAsync();

        await _productService.GetProductsNormalQuery("Fr", 6);
        await _productService.GetProductsComplexQuery("Fr", 6);

        var iterations = 1000;
        var normalQueryTimes = new List<long>();
        var compiledQueryTimes = new List<long>();

        for (int i = 0; i < iterations; i++)
        {
            // Test normal query
            var normalStart = Stopwatch.StartNew();
            //await _productService.GetAllProductsAsync();
            await _productService.GetProductsNormalQuery("Fr", 6);
            normalQueryTimes.Add(normalStart.ElapsedMilliseconds);

            // Test compiled query
            var compiledStart = Stopwatch.StartNew();
            //await _productService.GetAllProductsCompiledAsync();
            await _productService.GetProductsComplexQuery("Fr", 6);
            compiledQueryTimes.Add(compiledStart.ElapsedMilliseconds);
        }

        // Calculate statistics
        var normalAvg = normalQueryTimes.Average();
        var compiledAvg = compiledQueryTimes.Average();
        var normalMax = normalQueryTimes.Max();
        var compiledMax = compiledQueryTimes.Max();
        var normalMin = normalQueryTimes.Min();
        var compiledMin = compiledQueryTimes.Min();

        _logger.LogInformation(
           """
            Benchmark Results ({iterations} iterations):
            Normal Query:
                Average: {normalAvg:F2}ms
                Min: {normalMin}ms
                Max: {normalMax}ms
            
            Compiled Query:
                Average: {compiledAvg:F2}ms
                Min: {compiledMin}ms
                Max: {compiledMax}ms
            
            Performance Improvement: {improvement:F2}%
            """,
           iterations,
           normalAvg,
           normalMin,
           normalMax,
           compiledAvg,
           compiledMin,
           compiledMax,
           ((normalAvg - compiledAvg) / normalAvg) * 100);

    }
}
