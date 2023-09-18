using CommunicationService.Database;
using CommunicationService.Events;
using CommunicationService.Models.Options;
using CommunicationService.RepoService;
using CommunicationService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOption<AppOption>();
// Add services to the container.
builder.Services.AddDbContext<ProductDatabaseContext>(options =>
{
    var dbString = builder.Configuration.GetConnectionString("myConnectionString");
    options.UseSqlServer(dbString);
});
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<SubscriberController>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
