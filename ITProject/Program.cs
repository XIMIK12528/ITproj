using Serilog;

try
{
    Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

    var host = CreateHostBuilder(args).Build();
    //await host.MigrateDatabaseAsync<PlatypusContext>();
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Stopped program because of an exception!");
}
finally
{
    await Log.CloseAndFlushAsync();
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    var webHost = Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

    return webHost;
}