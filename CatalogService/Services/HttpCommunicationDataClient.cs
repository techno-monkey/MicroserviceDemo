using CatalogService.Database;
using System.Text;
using System.Text.Json;

namespace CatalogService.Services
{
    public class HttpCommunicationDataClient : ICommunicationDataClient
    {
        private readonly HttpClient _httpClient;

        public HttpCommunicationDataClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task SendCategoryToCommunication(CategoryDto category)
        {
            var relativeUri = new Uri("/api/CommunicationAPI/", UriKind.Relative);
            var httpContent = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            httpRequest.Content = httpContent;
            var response = await _httpClient.SendAsync(httpRequest);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("-- Post to communication service is done.");
            }
            else
                Console.WriteLine("-- Post to communication service is failed.");
        }
    }
}
