using RabbitMQ.Producer.Api.DependencyInjection.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Add serilog configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Host.UseSerilog();

// Add authorization
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

//Add Masstransit RabbitMq Configuration
builder.Services.AddConfigureMasstransitRabbitMQ(builder.Configuration);

//Add MediatR
builder.Services.AddMediat();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();