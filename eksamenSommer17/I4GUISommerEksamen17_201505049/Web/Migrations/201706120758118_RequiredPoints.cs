namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredPoints : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BackLogs", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.BackLogs", "Accountability", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BackLogs", "Accountability", c => c.String());
            AlterColumn("dbo.BackLogs", "Description", c => c.String());
        }
    }
}
