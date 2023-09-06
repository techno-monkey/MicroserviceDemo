using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ProductService.Database;
using ProductService.Models.Options;
using ProductService.Services;

namespace ProductService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOption<CatalogOption>();
            services.AddDbContext<ProductDatabaseContext>(options =>
            {
                var dbString = Configuration.GetConnectionString("myConnectionString");
                options.UseSqlServer(dbString);
            });
            services.AddHttpClient<ICatalogService, CatalogService>("Catalog", (serviceProvider, client) =>
            {
                var catalogOption = serviceProvider.GetRequiredService<IOptions<CatalogOption>>().Value;
                client.BaseAddress = new Uri(catalogOption.Url);
            });
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            { //<-- NOTE 'Add' instead of 'Configure'
                c.SwaggerDoc("v3", new OpenApiInfo
                {
                    Title = "Product",
                    Version = "v3"
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v3/swagger.json", "CatalogService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }



}
