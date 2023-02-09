
using App.WorkerApp;
using Microsoft.Extensions.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext ,services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        ConnectionStrings.DbSalesPrice = configuration.GetConnectionString("DbSalesPrice");
        ConnectionStrings.RedisUrl = configuration.GetConnectionString("RedisUrl");
        WorkerProps.Time = configuration.GetValue<int>("WorkerProps:Time");
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
