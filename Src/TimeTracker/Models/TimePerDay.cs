using System;
using System.Collections.Generic;

namespace TimeTracker.Models
{
    public partial class TimePerDay
    {
        public int Id { get; set; }
        public System.DateTime WorkedOn { get; set; }
        public double WorkedHours { get; set; }
        public Nullable<double> HourStarted { get; set; }
        public string JobCategoryId { get; set; }
        public Nullable<int> TaskItemId { get; set; }
        public string Note { get; set; }
        public Nullable<int> InvoiceId { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual lkuJobCategory lkuJobCategory { get; set; }
				//public virtual TaskItem TaskItem { get; set; }
    }
}
