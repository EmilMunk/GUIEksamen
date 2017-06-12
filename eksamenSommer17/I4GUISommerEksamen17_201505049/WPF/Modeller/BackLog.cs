using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Modeller
{
    public class BackLog 
    {
        public int BackLogId { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public double EstimatedTime { get; set; }
        public string Responsible { get; set; }
        public enum State { Backlog, IsToDo, IsDoing, Done }
        public State States { get; set; }
    }

}
