namespace Db.TimeTrack.DbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

	[Table("Invoice")]
    public partial class Invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            TimePerDays = new HashSet<TimePerDay>();
            TimeIntervals = new HashSet<TimeInterval>();
        }

        public int Id { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime PeriodFrom { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime PeriodUpTo { get; set; }

        public decimal TotalHours { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal HstPercent { get; set; }

        [Column(TypeName = "money")]
        public decimal RateSubmitted { get; set; }

        public bool IsSubmitted { get; set; }

        public bool HasCleared { get; set; }

        public int? InvoiceeId { get; set; }

        [StringLength(512)]
        public string Notes { get; set; }

        public virtual Invoicee Invoicee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimePerDay> TimePerDays { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeInterval> TimeIntervals { get; set; }
    }
}
