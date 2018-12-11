namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bill",
                c => new
                    {
                        BillID = c.String(nullable: false, maxLength: 10, unicode: false),
                        CustomerName = c.String(nullable: false, maxLength: 250),
                        DeliveryAddress = c.String(nullable: false, storeType: "ntext"),
                        Phone = c.String(nullable: false, maxLength: 11, unicode: false),
                        TotalPrice = c.Int(nullable: false),
                        Note = c.String(storeType: "ntext"),
                        Status = c.Boolean(nullable: false),
                        UserID = c.String(maxLength: 50, unicode: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.BillID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        BillID = c.String(nullable: false, maxLength: 10, unicode: false),
                        ProdID = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.BillID, t.ProdID })
                .ForeignKey("dbo.Bill", t => t.BillID, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProdID, cascadeDelete: true)
                .Index(t => t.BillID)
                .Index(t => t.ProdID);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProdID = c.Int(nullable: false, identity: true),
                        ProdName = c.String(nullable: false, maxLength: 200),
                        Code = c.String(nullable: false, maxLength: 230, unicode: false),
                        Image = c.Binary(storeType: "image"),
                        ImageUrl = c.String(maxLength: 4000),
                        Decription = c.String(storeType: "ntext"),
                        Cost = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        CateID = c.Byte(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProdID)
                .ForeignKey("dbo.Category", t => t.CateID, cascadeDelete: true)
                .Index(t => t.CateID);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CateID = c.Byte(nullable: false, identity: true),
                        CodeName = c.String(maxLength: 150, unicode: false),
                        CateName = c.String(nullable: false, maxLength: 150),
                        isActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.CateID);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        ComID = c.String(nullable: false, maxLength: 10, unicode: false),
                        Content = c.String(nullable: false, storeType: "ntext"),
                        UserID = c.String(nullable: false, maxLength: 50, unicode: false),
                        ProdId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ComID)
                .ForeignKey("dbo.Product", t => t.ProdId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.ProdId);
            
            CreateTable(
                "dbo.Reply",
                c => new
                    {
                        ComID = c.String(nullable: false, maxLength: 10, unicode: false),
                        RepNo = c.Int(nullable: false),
                        Content = c.String(nullable: false, storeType: "ntext"),
                        UserID = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.ComID, t.RepNo })
                .ForeignKey("dbo.Comment", t => t.ComID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.ComID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 50, unicode: false),
                        Password = c.String(nullable: false, maxLength: 30, unicode: false),
                        FullName = c.String(nullable: false, maxLength: 250),
                        Address = c.String(nullable: false, storeType: "ntext"),
                        Phone = c.String(nullable: false, maxLength: 11),
                        Email = c.String(maxLength: 50),
                        GrantID = c.Byte(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Grant", t => t.GrantID, cascadeDelete: true)
                .Index(t => t.GrantID);
            
            CreateTable(
                "dbo.Grant",
                c => new
                    {
                        GrantID = c.Byte(nullable: false, identity: true),
                        GrantName = c.String(nullable: false, maxLength: 150),
                        isActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.GrantID);
            
            CreateTable(
                "dbo.Rating",
                c => new
                    {
                        RatID = c.String(nullable: false, maxLength: 10, unicode: false),
                        ProdID = c.Int(nullable: false),
                        Content = c.String(storeType: "ntext"),
                        Level = c.Byte(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.RatID)
                .ForeignKey("dbo.Product", t => t.ProdID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.ProdID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "ProdID", "dbo.Product");
            DropForeignKey("dbo.Reply", "UserID", "dbo.User");
            DropForeignKey("dbo.Rating", "UserID", "dbo.User");
            DropForeignKey("dbo.Rating", "ProdID", "dbo.Product");
            DropForeignKey("dbo.User", "GrantID", "dbo.Grant");
            DropForeignKey("dbo.Comment", "UserID", "dbo.User");
            DropForeignKey("dbo.Bill", "UserID", "dbo.User");
            DropForeignKey("dbo.Reply", "ComID", "dbo.Comment");
            DropForeignKey("dbo.Comment", "ProdId", "dbo.Product");
            DropForeignKey("dbo.Product", "CateID", "dbo.Category");
            DropForeignKey("dbo.Order", "BillID", "dbo.Bill");
            DropIndex("dbo.Rating", new[] { "UserID" });
            DropIndex("dbo.Rating", new[] { "ProdID" });
            DropIndex("dbo.User", new[] { "GrantID" });
            DropIndex("dbo.Reply", new[] { "UserID" });
            DropIndex("dbo.Reply", new[] { "ComID" });
            DropIndex("dbo.Comment", new[] { "ProdId" });
            DropIndex("dbo.Comment", new[] { "UserID" });
            DropIndex("dbo.Product", new[] { "CateID" });
            DropIndex("dbo.Order", new[] { "ProdID" });
            DropIndex("dbo.Order", new[] { "BillID" });
            DropIndex("dbo.Bill", new[] { "UserID" });
            DropTable("dbo.Rating");
            DropTable("dbo.Grant");
            DropTable("dbo.User");
            DropTable("dbo.Reply");
            DropTable("dbo.Comment");
            DropTable("dbo.Category");
            DropTable("dbo.Product");
            DropTable("dbo.Order");
            DropTable("dbo.Bill");
        }
    }
}
