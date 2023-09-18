using CommunicationService.Database;

namespace CommunicationService.RepoService
{
    public interface IProductRepo
    {
        bool SaveChanges();
        IEnumerable<Product> GetAllProducts();
        Product GetProduct(int id);
        void CreateProduct(Product product);

    }

    public class ProductRepo : IProductRepo
    {
        private readonly ProductDatabaseContext _dbContext;
        public ProductRepo(ProductDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void CreateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException();
            }
            _dbContext.Products.Add(product);
        }

        public IEnumerable<Product> GetAllProducts() => _dbContext.Products;

        public Product GetProduct(int id) => _dbContext.Products.Where(c => c.ProductId == id).FirstOrDefault();

        public bool SaveChanges() => _dbContext.SaveChanges() > 0;
    }

}
