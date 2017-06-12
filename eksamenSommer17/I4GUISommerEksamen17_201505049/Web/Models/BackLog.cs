using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class BackLog
    {
        public int BackLogId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(1,4)]
        public int Priority { get; set; }
        [Required]
        public double EstimatedTime { get; set; }
        [Required]
        public string Accountability { get; set; }
        public bool IsToDo { get; set; }
        public bool IsDoing { get; set; }
        public bool Review { get; set; }
        public bool Done { get; set; }
    }
}