using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Pokerino.Shared.Entities;

namespace Pokerino.Server.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionString") ?? Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<RoomUser> RoomUser { get; set; }

        public DbSet<RoomTopic> RoomTopic { get; set; }

        public DbSet<EstimateVote> EstimateVote { get; set; }
    }
}

