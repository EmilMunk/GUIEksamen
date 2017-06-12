namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lavetEnum : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BackLogs", "Accountability");
            DropColumn("dbo.BackLogs", "IsToDo");
            DropColumn("dbo.BackLogs", "IsDoing");
            DropColumn("dbo.BackLogs", "Review");
            DropColumn("dbo.BackLogs", "Done");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BackLogs", "Done", c => c.Boolean(nullable: false));
            AddColumn("dbo.BackLogs", "Review", c => c.Boolean(nullable: false));
            AddColumn("dbo.BackLogs", "IsDoing", c => c.Boolean(nullable: false));
            AddColumn("dbo.BackLogs", "IsToDo", c => c.Boolean(nullable: false));
            AddColumn("dbo.BackLogs", "Accountability", c => c.String(nullable: false));
        }
    }
}
