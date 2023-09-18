using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Database
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CategoryId { get; set; }

        [Required]
        public String Name { get; set; }
    }


    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public String Name { get; set; }
    }

    public class CategoryPublishDto
    {
        public int id { get; set; }
        public string Name { get; set; }

        public string Event { get; set; }
    }


}
