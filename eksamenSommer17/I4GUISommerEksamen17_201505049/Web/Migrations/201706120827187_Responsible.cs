namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Responsible : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BackLogs", "Responsible", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BackLogs", "Responsible");
        }
    }
}
