using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using DataAccsess.Context;
using Domain.Interfaces.Repositories;
using Repositories;
using Services.ScheduleScraperService;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();

services.AddDbContext<Context>(options =>
    options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

services.AddScoped<IScheduleRepository, ScheduleRepository>();
services.AddScoped<ScheduleScraperService>();

var provider = services.BuildServiceProvider();

using var scope = provider.CreateScope();
var scraper = scope.ServiceProvider.GetRequiredService<ScheduleScraperService>();

Console.WriteLine("Запуск парсера...");
await scraper.StartAsync();
Console.WriteLine("Завершено.");