namespace AgentMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartnerId = c.Int(nullable: false),
                        FeaturedDeal = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Thumbnail = c.String(),
                        Image = c.String(),
                        Image2 = c.String(),
                        Image3 = c.String(),
                        ProductName = c.String(maxLength: 20),
                        DescriptionHook1 = c.String(maxLength: 50),
                        MainDescription = c.String(maxLength: 128),
                        BulletPoint1 = c.String(maxLength: 50),
                        BulletPoint2 = c.String(maxLength: 50),
                        BulletPoint3 = c.String(maxLength: 50),
                        BulletPoint4 = c.String(maxLength: 50),
                        BulletPoint5 = c.String(maxLength: 50),
                        BulletPoint6 = c.String(maxLength: 50),
                        DescriptionHook2 = c.String(maxLength: 50),
                        DescriptionLong = c.String(),
                        OriginalPrice = c.Single(nullable: false),
                        SalePrice = c.Single(nullable: false),
                        PostDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        DynamicMenuItem_Id = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DynamicMenuItems", t => t.DynamicMenuItem_Id, cascadeDelete: true)
                .Index(t => t.DynamicMenuItem_Id);
            
            AddColumn("dbo.Comments", "Product_Id", c => c.Int());
            CreateIndex("dbo.Comments", "Product_Id");
            AddForeignKey("dbo.Comments", "Product_Id", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "DynamicMenuItem_Id", "dbo.DynamicMenuItems");
            DropForeignKey("dbo.Comments", "Product_Id", "dbo.Products");
            DropIndex("dbo.Products", new[] { "DynamicMenuItem_Id" });
            DropIndex("dbo.Comments", new[] { "Product_Id" });
            DropColumn("dbo.Comments", "Product_Id");
            DropTable("dbo.Products");
        }
    }
}
