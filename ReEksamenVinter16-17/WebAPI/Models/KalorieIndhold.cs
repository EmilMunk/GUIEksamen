using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPI.Models
{
    public class KalorieIndhold
    {
        public int ID { get; set; }
        public string MadVare { get; set; }
        public string Energi { get; set; }
    }

    public class KalorieIndtag
    {
        public int ID { get; set; }
        public DateTime Day { get; set; }
        public int Indtag { get; set; }
    }

    public class Indtag
    {
        public int ID { get; set; }
        public DateTime Day { get; set; }
        public string MadVare { get; set; }
        public int Amount { get; set; }
        public double Kalorier { get; set; }
    }
}