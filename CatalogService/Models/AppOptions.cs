namespace CatalogService.Models
{
    public abstract class OptionBase
    {
        public abstract string OptionsName { get; }
    }

    public static class OptionConfigurationExtensions
    {
        public static void AddOption<TOptionBase>(this IServiceCollection services) where TOptionBase : OptionBase
        {
            services.AddOptions<TOptionBase>().
                Configure<IConfiguration>((settings, config) =>
                {
                    config.GetSection(typeof(TOptionBase).Name).Bind(settings);
                });
        }
    }

    public class AppOptions : OptionBase
    {
        public override string OptionsName => "AppOptions";
        public string Url { get; set; }

        public string RabbitMQHost { get; set; }

        public string RabbitMQPort { get; set; }
        public string ServiceBusConnectionString { get; set; }
        public string ServiceBusQueueName { get; set; }
    }
}
