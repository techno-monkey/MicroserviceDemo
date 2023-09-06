using Newtonsoft.Json;
using ProductService.Database;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public interface ICatalogService
    {
        Task<CategoryDto> GetCategoty(string name);
    }

    public class CatalogService : ICatalogService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CatalogService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }
        public async Task<CategoryDto> GetCategoty(string name)
        {
            var client = _httpClientFactory.CreateClient("Catalog");
            var response = await client.GetAsync($"/api/CatalogAPI/name/{name}");
            var apiContet = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<CategoryDto>(apiContet);
            return res;
        }

    }
}
