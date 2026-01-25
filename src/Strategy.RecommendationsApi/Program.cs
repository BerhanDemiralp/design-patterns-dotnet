using strategy.Services;
using strategy.Strategies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<FakeData>();
builder.Services.AddScoped<RecommendationService>();

builder.Services.AddScoped<IRecommendationStrategy, PopularStrategy>();
builder.Services.AddScoped<IRecommendationStrategy, NewStrategy>();
builder.Services.AddScoped<IRecommendationStrategy, PersonalizedStrategy>();
builder.Services.AddScoped<IRecommendationStrategy, CategoryStrategy>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
