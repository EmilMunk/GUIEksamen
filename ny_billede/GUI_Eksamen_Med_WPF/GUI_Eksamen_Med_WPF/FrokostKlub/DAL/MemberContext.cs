using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FrokostKlub.Models;

namespace FrokostKlub.DAL
{
    public class MemberContext : DbContext
    {
        public MemberContext() : base("DefaultConnection")
        {
            
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<LunchMeeting> LunchMeetings { get; set; }
    }
}