using Microsoft.EntityFrameworkCore;

namespace ProductService.Database
{
    public class ProductDatabaseContext:DbContext
    {
        public ProductDatabaseContext(DbContextOptions<ProductDatabaseContext> options):base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
    }
}
