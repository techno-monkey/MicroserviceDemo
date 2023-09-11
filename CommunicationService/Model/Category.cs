using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommunicationService.Model
{
    public class Category
    {
        public int CategoryId { get; set; }
        public String Name { get; set; }
    }
}
