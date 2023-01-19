using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SignalR_Project.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }  
        public DbSet<Room> Rooms { get; set; }  
    }
}