using Factory.PaymentsApi.Factory;
using Factory.PaymentsApi.Providers;
using Factory.PaymentsApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Providers
builder.Services.AddScoped<IPaymentProvider, StripePaymentProvider>();
builder.Services.AddScoped<IPaymentProvider, PayPalPaymentProvider>();
builder.Services.AddScoped<IPaymentProvider, IyzicoPaymentProvider>();

// Factory + Service
builder.Services.AddScoped<PaymentProviderFactory>();
builder.Services.AddScoped<PaymentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
