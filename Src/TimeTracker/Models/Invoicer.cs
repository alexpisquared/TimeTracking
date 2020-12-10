using System;
using System.Collections.Generic;

namespace TimeTracker.Models
{
    public partial class Invoicer
    {
        public Invoicer()
        {
            this.DefaultSettings = new List<DefaultSetting>();
        }

        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string AddressDetails { get; set; }
        public string EmailAddress { get; set; }
        public virtual ICollection<DefaultSetting> DefaultSettings { get; set; }
    }
}
