using System;
using System.Collections.Generic;

namespace TimeTracker.Models
{
    public partial class DefaultSetting
    {
        public int Id { get; set; }
        public int CurrentInvoicerId { get; set; }
        public int CurrentInvoiceeId { get; set; }
        public decimal HstPercent { get; set; }
        public string XmlSomething { get; set; }
        public string Notes { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public double DayStartHour { get; set; }
        public double LunchStartHour { get; set; }
        public Nullable<int> DefaultTaskId { get; set; }
        public string DefaultJobCategoryId { get; set; }
        public string InvoiceSubFolder { get; set; }
        public virtual Invoicee Invoicee { get; set; }
        public virtual Invoicer Invoicer { get; set; }
    }
}
