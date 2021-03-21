using Microsoft.EntityFrameworkCore;
using Tea.Core.Domain;

namespace Tea.Core.Impl.Data
{
    public class TeaContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Drink> Drink { get; set; }

        public TeaContext(){}

        public TeaContext(DbContextOptions<TeaContext> options) : base(options)
        {
        }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Round>().ToTable("Round");
            modelBuilder.Entity<History>().ToTable("History");
            modelBuilder.Entity<RoundUser>().ToTable("RoundUser");
            modelBuilder.Entity<RoundDetail>().ToTable("RoundDetail");
            modelBuilder.Entity<Drink>().ToTable("Drink");

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(d => d.History)
                  .WithOne(p => p.User);
            });

            modelBuilder.Entity<Round>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(d => d.UsersInRound)
                    .WithOne();
                entity.HasMany(r => r.Rounds)
                    .WithOne();
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<RoundUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(u => u.Round)
                    .WithMany(x => x.UsersInRound);
            });

            modelBuilder.Entity<RoundDetail>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.HasOne(r => r.Round)
                    .WithMany(x => x.Rounds);
            });

            modelBuilder.Entity<Drink>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(e => e.Emoji).HasColumnType("text CHARACTER SET utf8");

                
            });
        }
    }
}
