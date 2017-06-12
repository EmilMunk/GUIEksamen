namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnumStateProp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BackLogs", "States", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BackLogs", "States");
        }
    }
}
