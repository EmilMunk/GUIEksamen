namespace FrokostKlub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class meetingname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LunchMeetings", "MeetingName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LunchMeetings", "MeetingName");
        }
    }
}
