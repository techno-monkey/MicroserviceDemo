namespace ProductService.Models.Options
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

    public class CatalogOption: OptionBase
    {
        public override string OptionsName => "CatalogOption";
        public string Url { get; set; }
    }
}
