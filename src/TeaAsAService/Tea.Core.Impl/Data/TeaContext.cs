using Microsoft.EntityFrameworkCore;
using Tea.Core.Entity;

namespace Tea.Core.Impl.Data
{
    public class TeaContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<History> History { get; set; }

        public TeaContext(DbContextOptions<TeaContext> options) : base(options)
        {
        }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<History>().ToTable("History");

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(d => d.History)
                  .WithOne(p => p.User);
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            
        }

        
    }
}
