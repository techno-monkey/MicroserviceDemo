using CatalogService.Database;
using CatalogService.Models;
using Common.MessageBusServices;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace CatalogService
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
            services.AddOption<AppOptions>();
            services.AddDbContext<DatabaseContext>(options =>
            {
                var dbString = Configuration.GetConnectionString("myConnectionString");
                options.UseSqlServer(dbString);
            });

            
            //services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddSingleton<IAzureBusService, AzureBusService>((serviceProvider) => {
                var option = serviceProvider.GetService<IOptions<AppOptions>>().Value;
                return new AzureBusService(option.ServiceBusConnectionString, option.ServiceBusQueueName);
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            { //<-- NOTE 'Add' instead of 'Configure'
                c.SwaggerDoc("v3", new OpenApiInfo
                {
                    Title = "Category",
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
