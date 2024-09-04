using Gridify;
using Microsoft.AspNetCore.Mvc;
using WebApiGridify.Contacts;
using WebApiGridify.Models;
using WebApiGridify.Repositorys;

namespace WebApiGridify.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _productRepository.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        public ActionResult GetProducts([FromQuery] GridifyQuery gridifyQuery)
        {
            var products = _productRepository.GetProducts(gridifyQuery);

            return Ok(products);
        }


        [HttpGet]
        public ActionResult GetFilteredProducts(String QueryString)
        {
            var custGridifyQuery = new GridifyQuery()
            {
                Filter = QueryString
            };

            var products = _productRepository.GetFilteredProducts(custGridifyQuery);

            return Ok(products);
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            _productRepository.AddProduct(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            _productRepository.UpdateProduct(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _productRepository.DeleteProduct(id);
            return NoContent();
        }
    }
}
