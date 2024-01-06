Microservice Architecture using Kafka using .net core API

Wiki link : https://github.com/jayamoorthi/InventoryMicro/wiki/Wiki-%E2%80%90-Inventory-Microservice

Inventory Stock management application api for update/ delete stock of product.  To publish event through inventory api call actions, background service to perform backend logic update/delete of products stock from consumer event in the kafka broker message queue. 
To practical implementation following items need to Installation in your system. 
•	Visual Studio 2022 
•	.NET 6
•	ASP NET 6.0 Runtime
•	SqlServer Database
•	Apache Kafka 
•	Java Runtime Environment (JRE)
•	Docker Desktop
You can download these are tools from the following sources: 
Visual Studio 2022: Download Visual Studio Tools — Install Free for Windows, Mac, Linux (microsoft.com)
Apache Kafka: Download Apache Kafka
JRE: Download Java for Windows

Let’s Open Visual Studio 2022
Create a new .net core Api projects along with Domain and Infrastructure Class Library project also for inventory maintains stocks. 
  
Local Environment setup for application development and running dependent images ( Kafka, Sql server, Zoopkeeper and KafkadropUI ) pull from Registry containers.    

Kafka Publisher/Consumer and Db Connection string configuration in the appsettings for Inventory.API & OutBox project.
 
Appsettings.Json 
"Kafka": {
    "BootstrapServers": "localhost:9092",
    "Topic": "InventoryStock",
    "GroupId" : "InventoryStockConsumerGroup"
  },
  "ConnectionStrings": {
    "TestDb": "Data Source=localhost,1433;Initial Catalog=InventoryDb;User Id=SA;Password=Moorthi@16;Integrated Security=false;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=True"
  }

Docker-compose.yml: 

 

Open Command window and go to root path of the project directory and run the command :  
docker-compose up -d 
 
then all the dependent container images started successfully. 
Then now, go to the docker desktop, see it’s running.




 

Add Models Folder and create InventoryStockRequest.cs model class for api request.
InventoryStockRequest.cs:

    public class InventoryStockRequest
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
    } 
Add a new api InventoryController.cs for the inventory stocks 

public class InventoryController : ControllerBase
    {
        private readonly ProducerService _producerService;
        private readonly IConfiguration _configuration;

        public InventoryController(ProducerService producerService, IConfiguration configuration)
        {
            _producerService = producerService;
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<IActionResult> InventoryStcok([FromBody] InventoryStockRequest request)
        {
            var message = JsonSerializer.Serialize(request);
            var topic = _configuration["Kafka:Topic"];
            await _producerService.ProduceAsync(topic, message);

            return Ok("Inventory Updated Successfully...");
        }
    }
Domain Models and EntityTypeConfiguration using Fluent Validation in Domain Project
public class InventoryStock : BaseEntity
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;
    }
  public class InventoryStockEntityConfiguration : IEntityTypeConfiguration<InventoryStock>
    {
        public void Configure(EntityTypeBuilder<InventoryStock> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x=> x.ProductId).IsRequired();
            builder.Property(x=> x.Price).IsRequired();
            builder.Property(x=> x.Quantity).IsRequired();
            builder.Property(x => x.CreatedAt);
            builder.Property(x => x.UpdatedAt);
        }
    }
Add Interfaces and Services folder for abstraction layer accessing through DI in application
InventoryDbContext.cs class to inherit base dbcontext from EntityFrameworkCore library. 

public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SellingEntityConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryStockEntityConfiguration());
        }

        public DbSet<InventoryStock> InventoryStock { get; set; }

        public DbSet<Selling> Selling { get; set; }
    }

EF core DB Migration using PMC 
Add-Migration Init
 
 
After successfully migration generated and Update-Database command to save change in the Local DB.
 


OutBox :
BackgroundService running for consumer receiving message from kafka broker queue. 

Implement background job service for consumer:
   We need to implement from BackgroundService class in BackgroundConsumerService.cs class
 
public class BackgroundConsumerService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BackgroundConsumerService> _logger;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BackgroundConsumerService(IConfiguration configuration, ILogger<BackgroundConsumerService> logger,
             IServiceScopeFactory serviceScopeFactory)
            
        {
            _configuration = configuration;
            _logger = logger;
            _serviceScopeFactory= serviceScopeFactory;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = _configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Ignore,string>(consumerConfig).Build();

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var topic = _configuration["Kafka:Topic"];
            _consumer.Subscribe(topic);
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessKafkaMessage(stoppingToken);
                Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _consumer.Close();
        }

        public async Task ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    InventoryStock inventoryStock = JsonSerializer.Deserialize<InventoryStock>(message);
                    var inventoryService = scope.ServiceProvider.GetRequiredService<IInventoryService>();
                    await inventoryService.SaveInventoryStockAsync(inventoryStock);
                }

                //
                _logger.LogInformation($"Received inventory update: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Kafka message: {ex.Message}");
            }
        }

    }
}
Configure DI services in the Program.cs file for AddHostedService<>() method. 
 

Before application running, we have to set startup multiple projects as API and OutBox console app. 
While Right click application and click Set Startup project option and click multiple startup projects Radio button  and set action as Start for those two (Inventory PAI & OutBox project) then click apply and Ok button. 
 
Swagger ui page loading for API documentation and  Outbox Command window for background job.
 
 

Request Payload: 
{
  "id": 0,
  "productId": "Lenova Laptop",
  "quantity": 10,
  "price": 1000000
}
 

 

Open KafkaDrop : Kafdrop: Broker List

 
Now, InventoryStock topic has one message in the Kefka broker queue. Also, we can able to open and see that message in the queue while click on that topic. 
 

 

 

Github Repo: 
Conclusion :
 This article could be very useful for beginner, to understand how to design and develop for Microservice architecture with practical implementation. 
    
