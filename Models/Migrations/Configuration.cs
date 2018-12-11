namespace Models.Migrations
{
    using EntityFramework;
    using Common;
    using Models.Common.Encode;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ShopDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            List<Grant> grants = new List<Grant> {
                new Grant{ GrantName = "Manager", isActive = true, CreatedAt = DateTime.Now },
                new Grant{ GrantName = "Staff", isActive = true , CreatedAt = DateTime.Now },
                new Grant{ GrantName = "Customer", isActive = true , CreatedAt = DateTime.Now }
            };

            foreach (var item in grants)
            {
                context.Grants.Add(item);
            }

            //Add Seed Data for Category
            List<Category> categories = new List<Category> {
                new Category{ CodeName = "coffee" ,CateName="Coffee",isActive=true, CreatedAt = DateTime.Now },
                new Category{ CodeName = "tra-&-macchiato" ,CateName="Trà & Macchiano",isActive=true, CreatedAt = DateTime.Now },
                new Category{ CodeName = "thuc-uong-trai-cay" ,CateName="Thức Uống Trái Cây",isActive=true, CreatedAt = DateTime.Now },
                new Category{ CodeName = "thuc-uong-da-xay" ,CateName="Thức Uống Đá Xay",isActive=true, CreatedAt = DateTime.Now },
            };

            foreach (var item in categories)
            {
                context.Categories.Add(item);
            }
        }
    }
}