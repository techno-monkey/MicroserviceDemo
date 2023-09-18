namespace CommunicationService.Models
{
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

    public class GenericEventDto
    {
        public string Event { get; set; }
    }
}
