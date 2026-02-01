using Decorator.CachingLoggingApi.Decorators;
using Decorator.CachingLoggingApi.Services;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<ProductCatalogService>();

builder.Services.AddScoped<IProductCatalogService>(sp =>
{
    // Core
    IProductCatalogService svc = sp.GetRequiredService<ProductCatalogService>();

    // Decorators
    svc = new CachingProductCatalogDecorator(svc, sp.GetRequiredService<IMemoryCache>());
    svc = new LoggingProductCatalogDecorator(svc, sp.GetRequiredService<ILogger<LoggingProductCatalogDecorator>>());

    return svc;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
