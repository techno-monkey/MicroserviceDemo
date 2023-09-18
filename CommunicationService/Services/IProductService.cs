using AutoMapper;
using CommunicationService.Database;
using CommunicationService.Models;
using CommunicationService.RepoService;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationService.Services
{
    public interface IProductService
    {
        ProductDto Create(Product product);
        IEnumerable<ProductDto> GetProducts();
        ProductDto GetProduct(int id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;
        public ProductService(IProductRepo productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }
        public ProductDto Create(Product product)
        {
            var id = _productRepo.GetAllProducts().Count() > 0 ? _productRepo.GetAllProducts().Max(c => c.ProductId) : 0;
            product.ProductId = ++id;
            product.ProductName = string.Concat(product.ProductName, id);
            _productRepo.CreateProduct(product);
            _productRepo.SaveChanges();
            return _mapper.Map<ProductDto>(product);
        }

        public IEnumerable<ProductDto> GetProducts()
        {
            var products = _productRepo.GetAllProducts();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public ProductDto GetProduct(int id)
        {
            var product = _productRepo.GetProduct(id);
            return _mapper.Map<ProductDto>(product);
        }
    }
}
