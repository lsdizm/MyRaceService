using my.race.connect;
using my.race.worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<IDataAPI, DataAPI>();
        services.AddSingleton<IDataBases, DataBases>();
        services.AddSingleton<IConfigurations, Configurations>();
    })
    .Build();

await host.RunAsync();
