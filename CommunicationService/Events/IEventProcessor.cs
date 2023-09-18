using AutoMapper;
using CommunicationService.Database;
using CommunicationService.Models;
using CommunicationService.Services;
using System.Text.Json;

namespace CommunicationService.Events
{
    public interface IEventProcessor
    {
        void PorocessEvent(string msg);
    }

    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void PorocessEvent(string msg)
        {
            var eventType = GetEventType(msg);
            switch(eventType)
            {
                case EventType.CategoryPublished:
                    addProduct(msg);
                    break;
                default:
                    break;
            }
        }

        private void addProduct(string msg)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var productService = scope.ServiceProvider.GetRequiredService<IProductService>();

                var category = JsonSerializer.Deserialize<CategoryPublishDto>(msg);
                var product = new Product { CategoryId = category.id, ProductName = "Product" };
                try
                {
                    productService.Create(product);
                    Console.WriteLine("--> Product added!");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
                }
            }
        }

        private EventType GetEventType(string msg)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(msg);
            switch (eventType.Event)
            {
                case "CategoryPublish":
                    Console.WriteLine("CategoryPublish event found.");
                    return EventType.CategoryPublished;
                default:
                    Console.WriteLine("Non CategoryPublish event found.");
                    return EventType.Unknow;

            }
        }
    }

    enum EventType
    {
        CategoryPublished,
        Unknow
    }
}
