using Microsoft.EntityFrameworkCore;

namespace MessageBoard.Models
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions options) : base(options) {}

        // DBSets!!!!
            
        public DbSet<Post> Posts {get;set;}
        public DbSet<User> Users {get;set;}
        public DbSet<Vote> Votes {get;set;}
    }
}