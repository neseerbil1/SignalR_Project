using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class Context
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
