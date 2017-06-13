namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BackLogs",
                c => new
                    {
                        BackLogId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Priority = c.Int(nullable: false),
                        EstimatedTime = c.Double(nullable: false),
                        Accountability = c.String(),
                        IsToDo = c.Boolean(nullable: false),
                        IsDoing = c.Boolean(nullable: false),
                        Review = c.Boolean(nullable: false),
                        Done = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BackLogId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BackLogs");
        }
    }
}
