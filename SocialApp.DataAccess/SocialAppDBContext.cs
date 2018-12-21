using Microsoft.EntityFrameworkCore;
using SocialApp.Domain; 

namespace SocialApp.DataAccess
{
    public class SocialAppDbContext : DbContext
    {
        public SocialAppDbContext(DbContextOptions<SocialAppDbContext> options) : base (options) { }
        
        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}
