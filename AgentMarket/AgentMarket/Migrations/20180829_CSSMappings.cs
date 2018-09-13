namespace AgentMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CSSMappings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CSSMappingEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CSSMapping_Id = c.Int(nullable: false),
                        Value = c.String(),
                        DynamicMenuItem_Id = c.Short(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CSSMappings", t => t.CSSMapping_Id, cascadeDelete: true)
                .ForeignKey("dbo.DynamicMenuItems", t => t.DynamicMenuItem_Id)
                .Index(t => t.CSSMapping_Id)
                .Index(t => t.DynamicMenuItem_Id);
            
            CreateTable(
                "dbo.CSSMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PrettyName = c.String(),
                        CSSName = c.String(),
                        CSSProperty = c.String(),
                        CSSUnit = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CSSMappingEntries", "DynamicMenuItem_Id", "dbo.DynamicMenuItems");
            DropForeignKey("dbo.CSSMappingEntries", "CSSMapping_Id", "dbo.CSSMappings");
            DropIndex("dbo.CSSMappingEntries", new[] { "DynamicMenuItem_Id" });
            DropIndex("dbo.CSSMappingEntries", new[] { "CSSMapping_Id" });
            DropTable("dbo.CSSMappings");
            DropTable("dbo.CSSMappingEntries");
        }
    }
}
