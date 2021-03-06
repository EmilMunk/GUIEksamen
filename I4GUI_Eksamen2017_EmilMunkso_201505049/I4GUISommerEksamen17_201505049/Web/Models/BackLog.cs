﻿using System;
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
        public string Responsible { get; set; }
        public enum State { Backlog, IsToDo, IsDoing, Done }
        public State States { get; set; }
    }
}