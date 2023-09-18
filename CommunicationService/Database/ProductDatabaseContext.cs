using Microsoft.EntityFrameworkCore;

namespace CommunicationService.Database
{
    public class ProductDatabaseContext:DbContext
    {
        public ProductDatabaseContext(DbContextOptions<ProductDatabaseContext> options):base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
    }
}
