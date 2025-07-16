
using RabbitMQ.Producer.Api.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add authorization
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

//Add Masstransit RabbitMq Configuration
builder.Services.AddConfigureMasstransitRabbitMQ(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
