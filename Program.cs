
using App.WorkerApp;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext ,services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        ConnectionStrings.DbSalesPrice = configuration.GetConnectionString("DbSalesPrice");
        ConnectionStrings.RedisUrl = configuration.GetConnectionString("RedisUrl");
        
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
