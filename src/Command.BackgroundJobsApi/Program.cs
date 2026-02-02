using Command.BackgroundJobsApi.Commands;
using Command.BackgroundJobsApi.Handlers;
using Command.BackgroundJobsApi.Infrastructure;
using Command.BackgroundJobsApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Core services
builder.Services.AddSingleton<JobStore>();
builder.Services.AddSingleton<ICommandQueue, InMemoryCommandQueue>();

builder.Services.AddScoped<CommandDispatcher>();

// Handlers (register each handler)
builder.Services.AddScoped<ICommandHandler<SendEmailCommand>, SendEmailCommandHandler>();
builder.Services.AddScoped<ICommandHandler<SendSmsCommand>, SendSmsCommandHandler>();
builder.Services.AddScoped<ICommandHandler<GenerateReportCommand>, GenerateReportCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UserRegisteredCommand>, UserRegisteredCommandHandler>();

// Background worker
builder.Services.AddHostedService<CommandWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
