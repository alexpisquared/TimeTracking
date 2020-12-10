using System;
using System.Collections.Generic;

namespace TimeTracker.Models
{
	public partial class Invoicee
	{
		public Invoicee()
		{
			this.DefaultSettings = new List<DefaultSetting>();
			this.Invoices = new List<Invoice>();
		}

		public int Id { get; set; }
		public string CompanyName { get; set; }
		public string AddressDetails { get; set; }
		public string EmailAddress { get; set; }
		public string EmailBody { get; set; }
		public decimal CorpRate { get; set; }
		public System.DateTime StartDate { get; set; }
		public Nullable<System.DateTime> LasttDate { get; set; }
		public string PayPeriodMode { get; set; }
		public Nullable<int> PayPeriodStart { get; set; }
		public Nullable<int> PayPeriodLength { get; set; }
		public decimal HoursPerPeriod { get; set; }
		public string WebUsername { get; set; }
		public string WebPassword { get; set; }
		public string Notes { get; set; }
		public virtual ICollection<DefaultSetting> DefaultSettings { get; set; }
		public virtual ICollection<Invoice> Invoices { get; set; }
		public virtual lkuPayPeriodMode lkuPayPeriodMode { get; set; }
	}
}
