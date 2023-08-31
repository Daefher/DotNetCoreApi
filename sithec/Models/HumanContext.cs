using Microsoft.EntityFrameworkCore;

namespace sithec.Models
{
    
    public class HumanContext : DbContext
    {
        public HumanContext(DbContextOptions<HumanContext> options) : base (options) { }
        public DbSet<humano> humanos { get; set; } = null!;
    }
}
