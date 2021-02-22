using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantRating.Data;
using RestaurantRating.Interfaces;
using RestaurantRating.Services;

[assembly: FunctionsStartup(typeof(RestaurantRating.Startup))]
namespace RestaurantRating
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var connectionString =
                Environment.GetEnvironmentVariable("SqlServerConnection");
            builder.Services.AddDbContext<DataContext>(x =>
            {
                x.UseSqlServer(connectionString
                    , options => options.EnableRetryOnFailure());
            }); builder.Services.AddTransient<ICrudService, CrudService>();
        }
    }
}
