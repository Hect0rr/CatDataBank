using Microsoft.EntityFrameworkCore;

namespace CatDataBank.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> appDbContextOptions) : base(appDbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(a => a.Email);

            modelBuilder.Entity<Cat>().HasIndex(a => a.Id);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Cat> Cats { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=catdatabank;Trusted_Connection=True;");
        }
    }
}