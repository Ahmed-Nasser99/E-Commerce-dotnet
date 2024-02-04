using Microsoft.EntityFrameworkCore;
using Platform.Model;

namespace Platform.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base()
        {
            
        }
        public DbSet<Brand> brands { get; set; }
        public DbSet<Category> categorys { get; set; }
        public DbSet<SubCategory> subCategorys { get; set; }
        public DbSet<Product> products { get; set; }
    }
}
