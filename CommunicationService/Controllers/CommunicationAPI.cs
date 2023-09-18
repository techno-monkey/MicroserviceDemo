using AutoMapper;
using CommunicationService.Database;
using CommunicationService.Models;
using CommunicationService.RepoService;
using CommunicationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationAPI : ControllerBase
    {
        private readonly IProductService _productService;

        public CommunicationAPI(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public ActionResult<ProductDto> CreateProduct(CategoryDto category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("category");
            }
            var product = new Product { CategoryId = category.CategoryId, ProductName = "Product" };
            return _productService.Create(product);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetProducts()
        {
            return Ok(_productService.GetProducts());
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetProduct(int id)
        {
            return Ok(_productService.GetProduct(id));
        }
    }
}
