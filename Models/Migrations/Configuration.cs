namespace Models.Migrations
{
    using EntityFramework;
    using Models.Common;
    using Models.Common.Encode;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Web;

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
                new Category{ CodeName = "do-uong-dong-chai" ,CateName="Đồ uống đóng chai",isActive=true, CreatedAt = DateTime.Now },
                new Category{ CodeName = "do-uong-trai-cay" ,CateName="Đồ uống trai cay",isActive=true, CreatedAt = DateTime.Now },
            };

            foreach (var item in categories)
            {
                context.Categories.Add(item);
            }

            // Add Seed Data for Product
            List < Product > products = new List<Product> {
                new Product{ ProdName = "Cafe đá xay" ,Code="cafe-da-xay",Decription="Cafe đá xay",Cost=25000,CateID=1,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/cafe-da-xay.jpg" },
                new Product{ ProdName = "Cafe đen đá" ,Code="cafe-den-da",Decription="Cafe đen đá",Cost=20000,CateID=1,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/cafe-den-da.png" },
                new Product{ ProdName = "Cafe sữa" ,Code="cafe-sua",Decription="Cafe sữa",Cost=24000,CateID=1,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/cafe-sua.jpg" },
                new Product{ ProdName = "Capuchino" ,Code="capuchino",Decription="Capuchino",Cost=26000,CateID=1,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/capuchino.jpg" },
                new Product{ ProdName = "Espresso" ,Code="espresso",Decription="Espresso",Cost=26000,CateID=1,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/espresso.png" },
                new Product{ ProdName = "Cafe Phin" ,Code="cafe-phin",Decription="Cafe Phin",Cost=20000,CateID=1,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/cafe-phin.jpg" },
                new Product{ ProdName = "Sữa chua đá" ,Code="sua-chua-da",Decription="Sữa chua đá",Cost=20000,CateID=1,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/sua-chua-da.jpg"  },
                new Product{ ProdName = "Bạc xỉu" ,Code="bac-xiu",Decription="Bạc xỉu",Cost=23000,CateID=1,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/bac-xiu.jpg"  },
                new Product{ ProdName = "Sting" ,Code="sting",Decription="Nước tăng lực Sting",Cost=12000,CateID=2,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/sting-chai.jpg"  },
                new Product{ ProdName = "Nuti" ,Code="nuti",Decription="Chai Nuti",Cost=16000,CateID=2,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/nuti.jpg"  },
                new Product{ ProdName = "NumberOne" ,Code="numberone",Decription="Chai Number One",Cost=15000,CateID=2,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/numberone-chai.jpg"  },
                new Product{ ProdName = "Trà xanh không độ" ,Code="tra-xanh-0-do",Decription="Chai Trà xanh 0 độ",Cost=14000,CateID=2,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/tra-xanh-0do.jpg"  },
                new Product{ ProdName = "7up Revive" ,Code="7up-revive",Decription="Chai 7up Revive",Cost=15000,CateID=2,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/revive-chai.jpg"  },
                new Product{ ProdName = "Nước khoáng vĩnh hảo" ,Code="khoang-vinh-hao",Decription="Nước khoáng vĩnh hảo",Cost=10000,CateID=2,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/vinh-hao.png"  },
                new Product{ ProdName = "Redbull" ,Code="redbull",Decription="Nước tăng lực RedBull",Cost=18000,CateID=2,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/redbull.jpg"  },
                new Product{ ProdName = "Trà đào" ,Code="tra-dao",Decription="Trà đào",Cost=22000,CateID=3,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/tra-dao.jpg"  },
                new Product{ ProdName = "Trà gừng" ,Code="tra-gung",Decription="Trà gừng",Cost=22000,CateID=3,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/tra-gung.jpg"  },
                new Product{ ProdName = "Nước mía" ,Code="nuoc-mia",Decription="Nước mía",Cost=10000,CateID=3,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/nuoc-mia.jpg"  },
                new Product{ ProdName = "Nước ép carot" ,Code="nuoc-ep-carot",Decription="Nước ép carot",Cost=15000,CateID=3,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/nuoc-ep-carot.jpg"  },
                new Product{ ProdName = "Nước ép thơm" ,Code="nuoc-ep-thom",Decription="Nước ép thom",Cost=15000,CateID=3,isActive=true, CreatedAt = DateTime.Now,ImageUrl="/assets/Data/Images/images/thom-ep.jpg"  },
            };

            foreach (var item in products)
                {
                    var path = HttpContext.Current.Server.MapPath($"~/{item.ImageUrl}");
                    item.Image = Converter.imageToByteArray(path);
                    context.Products.Add(item);
                }

            //Add Seed Data for User with password='1234'
            List<User> users = new List<User> {
                new User{ UserID = "hieult22" ,Password="4a/q3di+hRwSBfNaRDLXig==",FullName="Lương Trung Hiếu",Address="P3.HCM",Phone="0344158136",Email="HieuLT22@fsoft.com.vn",GrantID=1,isActive=true, CreatedAt = DateTime.Now },
                new User{ UserID = "admin" ,FullName="Nguyễn Văn A",Address="Quận 1, TP.HCM",Phone="0564851544",Email="nguyenvana@gmail.com",GrantID=1,isActive=true, CreatedAt = DateTime.Now },
                new User{ UserID = "user" ,FullName="Nguyễn Văn B",Address="Quận 12, TP.HCM",Phone="0564853244",Email="nguyenvanb@gmail.com",GrantID=2,isActive=true, CreatedAt = DateTime.Now },
                new User{ UserID = "user1" ,FullName="Nguyễn Văn C",Address="Quận 8, TP.HCM",Phone="0543853244",Email="nguyenvanhan@gmail.com",GrantID=3,isActive=true, CreatedAt = DateTime.Now },
                new User{ UserID = "user2" ,FullName="Nguyễn Văn D",Address="Quận 2, TP.HCM",Phone="0543863244",Email="nguyenvand@gmail.com",GrantID=3,isActive=true, CreatedAt = DateTime.Now }
            };

            foreach (var item in users)
            {
                if (item.UserID != "hieult22")
                {
                    item.Password = Encrypt.Encrypt_Code("admin123qwe", Constants.trueValue);
                }
                context.Users.Add(item);
            }
        }
    }
}