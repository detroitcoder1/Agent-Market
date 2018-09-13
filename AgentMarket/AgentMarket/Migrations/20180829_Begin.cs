namespace AgentMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Begin : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "PostDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Items", "PostDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Items", "PostDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Comments", "PostDate", c => c.DateTime(nullable: false));
        }
    }
}
