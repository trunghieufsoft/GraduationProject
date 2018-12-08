using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Models.EntityFramework
{
    public partial class ShopDbContext : DbContext
    {
        public ShopDbContext()
            : base("name=ShopDbContext")
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public DbSet<Bill> Bills { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public DbSet<Grant> Grants { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<User>().HasMany(t => t.Replies).WithRequired(a => a.User).WillCascadeOnDelete(false);
        }
    }
}