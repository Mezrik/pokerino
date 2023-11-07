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
            if (Environment.GetEnvironmentVariable("DATABASE_URL") is not null)
            {
                Uri url;
                bool isUrl = Uri.TryCreate(Environment.GetEnvironmentVariable("DATABASE_URL"), UriKind.Absolute, out url);

                if (url is not null)
                    options.UseNpgsql($"Host={url.Host}; Database={url.LocalPath.Substring(1)}; Username={url.UserInfo.Split(':')[0]}; Include Error Detail=True; Password={url.UserInfo.Split(':')[1]}; SSL Mode=Disable;");
            }
            else
            {
                options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
            }
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<RoomUser> RoomUser { get; set; }

        public DbSet<RoomTopic> RoomTopic { get; set; }

        public DbSet<EstimateVote> EstimateVote { get; set; }
    }
}

