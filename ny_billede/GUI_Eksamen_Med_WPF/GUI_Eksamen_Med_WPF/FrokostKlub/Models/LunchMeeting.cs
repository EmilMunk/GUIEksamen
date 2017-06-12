using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FrokostKlub.Models
{
    public class LunchMeeting
    {
        [Key]
        public int MeetingId { get; set; }

        public string MeetingName { get; set; }
        public DateTime Date { get; set; }
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member Members { get; set; }
    }
}