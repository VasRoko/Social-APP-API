using Microsoft.EntityFrameworkCore;
using SocialApp.Domain; 

namespace SocialApp.DataAccess
{
    public class SocialAppDbContext : DbContext
    {
        public SocialAppDbContext(DbContextOptions<SocialAppDbContext> options) : base (options) { }
        
        // Values Table
        public DbSet<Value> Values { get; set; }
    }
}
