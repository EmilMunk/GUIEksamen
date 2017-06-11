using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class Member 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNr { get; set; }

        public virtual List<Meeting> Meetings { get; set; }

    }

    public class Meeting
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }
    }
}