using System;
using System.Collections.Generic;

namespace TimeTracker.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            this.TimePerDays = new List<TimePerDay>();
						//this.TimeIntervals = new List<TimeInterval>();
        }

        public int Id { get; set; }
        public System.DateTime PeriodFrom { get; set; }
        public System.DateTime PeriodUpTo { get; set; }
        public decimal TotalHours { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public decimal HstPercent { get; set; }
        public decimal RateSubmitted { get; set; }
        public bool IsSubmitted { get; set; }
        public bool HasCleared { get; set; }
        public Nullable<int> InvoiceeId { get; set; }
				public string Notes { get; set; }
				public virtual Invoicee Invoicee { get; set; }
        public virtual ICollection<TimePerDay> TimePerDays { get; set; }
				//public virtual ICollection<TimeInterval> TimeIntervals { get; set; }
    }
}
