using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrokostKlub.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string Name { get; set; }
        public string PhoneNr { get; set; }
        public virtual List<LunchMeeting> MeetingsAttended { get; set; }
    }
}