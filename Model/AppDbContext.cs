using Microsoft.EntityFrameworkCore;

namespace CatDataBank.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            Users.Add(new User {
                Id = 1,
                Email = "test@test.com",
                PasswordHash = null,
                PasswordSalt = null
            });
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(a => a.Email);
        }
     
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(optionsBuilder,"CatDataBank");
            //optionsBuilder.UseSqlServer("Server=ALIENWARE;Database=NergiePool;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}