namespace AgentMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmigrationupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false));
            
        }
        
        public override void Down()
        {
            
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String());
        }
    }
}
