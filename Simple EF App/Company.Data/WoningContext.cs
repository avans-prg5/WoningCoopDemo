
using Microsoft.EntityFrameworkCore;

namespace Company.Data
{
    public class WoningContext : DbContext
    {
        public DbSet<Woning> Woningen { get; set; } = null!;
        public DbSet<Bewoner> Bewoners { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=WoningCoopDb;Trusted_Connection=True;");   
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Seed data voor één-op-veel relatie
            modelBuilder.Entity<Woning>().HasData(
                new Woning() { Naam = "zelftevree", Id = 2 },
                new Woning() { Naam = "boslust", Id = 3 }
                );
            modelBuilder.Entity<Bewoner>().HasData(
                    new Bewoner { Naam = "Joosten", WoningId = 2, Id = 1 },
                    new Bewoner { Naam = "Verschoor", WoningId = 2, Id = 2 },
                    new Bewoner { Naam = "Rubens", WoningId = 3, Id = 3 },
                    new Bewoner { Naam = "Van Zanten", WoningId = 3, Id = 4 }
                );
        }
    }
}
