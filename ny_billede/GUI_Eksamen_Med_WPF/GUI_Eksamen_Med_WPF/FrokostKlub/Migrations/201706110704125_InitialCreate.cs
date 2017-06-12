namespace FrokostKlub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LunchMeetings",
                c => new
                    {
                        MeetingId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        MemberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MeetingId)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        MemberId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PhoneNr = c.String(),
                    })
                .PrimaryKey(t => t.MemberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LunchMeetings", "MemberId", "dbo.Members");
            DropIndex("dbo.LunchMeetings", new[] { "MemberId" });
            DropTable("dbo.Members");
            DropTable("dbo.LunchMeetings");
        }
    }
}
