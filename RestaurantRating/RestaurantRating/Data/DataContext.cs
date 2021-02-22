using Microsoft.EntityFrameworkCore;
using RestaurantRating.Models;


namespace RestaurantRating.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Restaurant> restaurant { get; set; }
        public DbSet<Rating> rating { get; set; }
    }
}
