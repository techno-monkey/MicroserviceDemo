using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Database
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
    }

}
