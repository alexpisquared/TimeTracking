using System;
using System.Collections.Generic;

namespace TimeTracker.Models
{
    public partial class lkuJobCategory
    {
        public lkuJobCategory()
        {
						//this.TimeIntervals = new List<TimeInterval>();
            this.TimePerDays = new List<TimePerDay>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsBillable { get; set; }
				//public virtual ICollection<TimeInterval> TimeIntervals { get; set; }
        public virtual ICollection<TimePerDay> TimePerDays { get; set; }
    }
}
