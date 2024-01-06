using kafka.ProductApi.EventHandler;
using kafka.ProductApi.Services;
using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Interfaces;
using ProductApi.Domain.Services;
using ProductApi.Infrastructure;
using ProductApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
string assemblyName = typeof(InventoryDbContext).Namespace;
var connectionString = configuration.GetConnectionString("TestDb");
builder.Services.AddTransient<IInventoryService, InventoryService>();
builder.Services.AddTransient<IInventoryRepository, InventoryRepository>();
builder.Services.AddDbContext<InventoryDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly(assemblyName)));
// Add services to the container.
builder.Services.AddSingleton<ProducerService>();
builder.Services.AddHostedService<BackgroundConsumerService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
