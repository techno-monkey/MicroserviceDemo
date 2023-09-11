using CatalogService.Database;

namespace CatalogService.Services
{
    public interface ICommunicationDataClient
    {
        Task SendCategoryToCommunication(CategoryDto category);
    }
}
