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
        public DbSet<Cart> Cart { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(x => x.id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Cart>().HasKey(t => new { t.ProductId, t.UserId });
            modelBuilder.Entity<Cart>().HasOne(ci => ci.Product)
                 .WithMany()
                 .HasForeignKey(ci => ci.ProductId);
            modelBuilder.Entity<Cart>().HasOne(ci => ci.UserId)
                .WithMany()
                .HasForeignKey(ci => ci.UserId);


        }
    }
}
