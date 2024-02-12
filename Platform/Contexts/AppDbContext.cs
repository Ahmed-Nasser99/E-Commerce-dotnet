using Microsoft.EntityFrameworkCore;
using Platform.Model;

namespace Platform.Contexts
{
    public class AppDbContext : DbContext
    {
         public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Brand> brands { get; set; }
        public DbSet<Category> categorys { get; set; }
        public DbSet<SubCategory> subCategorys { get; set; }
        public DbSet<Product> products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(x => x.id).HasDefaultValueSql("NEWID()");
        }
    }
}
