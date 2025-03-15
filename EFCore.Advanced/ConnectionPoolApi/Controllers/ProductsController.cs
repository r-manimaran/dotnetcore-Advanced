using ConnectionPoolApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ConnectionPoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        private readonly QueryBenchmark _queryBenckMark;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger, 
                QueryBenchmark queryBenckMark)
        {
            _productService = productService;
            _logger = logger;
            _queryBenckMark = queryBenckMark;
        }
        [HttpGet("normal/all")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("compiled/all")]
        public async Task<IActionResult> GetCompiledProducts()
        {
            var products = await _productService.GetAllProductsCompiledAsync();
            return Ok(products);
        }

        [HttpGet("normal/{id}")]
        public async Task<IActionResult> GetProductByIdNormal(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpGet("complied/{id}")]
        public async Task<IActionResult> GetProductByIdCompiled(Guid id)
        {
            var product = await _productService.GetProductByIdCompiledAsync(id);
            return Ok(product);
        }

        [HttpGet("normal/getrange")]
        public async Task<IActionResult> GetProductByPriceRangeNormal(decimal minPrice, decimal maxPrice)
        {
            var products = await _productService.GetProductByPriceRangeAsync(minPrice, maxPrice);
            return Ok(products);
        }

        [HttpGet("compiled/getrange")]
        public async Task<IActionResult> GetProductByPriceRangeCompiled(decimal minPrice, decimal maxPrice)
        {
            var products = await _productService.GetProductsByPriceRangeCompiledAsync(minPrice, maxPrice);
            return Ok(products);
        }
        [HttpGet("complex")]
        public async Task<IActionResult> GetProductsComplexCompiled(string namePattern, decimal minPrice)
        {
            var products = await _productService.GetProductsComplexQuery(namePattern, minPrice);
            return Ok(products);
        }

        [HttpGet("normalQry")]
        public async Task<IActionResult> GetProductsNormal(string namePattern, decimal minPrice)
        {
            var products = await _productService.GetProductsNormalQuery(namePattern, minPrice);
            return Ok(products);
        }

        [HttpGet("queryBenchmark")]
        public async Task<IActionResult> GetBenchMark()
        {
            // In your Program.cs or controller
            await _queryBenckMark.Benchmark();
            return Ok();
        }
    }
}
