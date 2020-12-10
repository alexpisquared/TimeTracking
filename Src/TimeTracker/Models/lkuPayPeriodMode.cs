using System;
using System.Collections.Generic;

namespace TimeTracker.Models
{
    public partial class lkuPayPeriodMode
    {
        public lkuPayPeriodMode()
        {
            this.Invoicees = new List<Invoicee>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Invoicee> Invoicees { get; set; }
    }
}
