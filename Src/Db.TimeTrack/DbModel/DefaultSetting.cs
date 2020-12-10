namespace Db.TimeTrack.DbModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

	[Table("DefaultSetting")]
    public partial class DefaultSetting
    {
        public int Id { get; set; }

        public int CurrentInvoicerId { get; set; }

        public int CurrentInvoiceeId { get; set; }

        [Column(TypeName = "money")]
        public decimal HstPercent { get; set; }

        public string XmlSomething { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        [StringLength(128)]
        public string CreatedBy { get; set; }

        public double DayStartHour { get; set; }

        public double LunchStartHour { get; set; }

        public int? DefaultTaskId { get; set; }

        [StringLength(3)]
        public string DefaultJobCategoryId { get; set; }

        [StringLength(128)]
        public string InvoiceSubFolder { get; set; }

        public virtual Invoicee Invoicee { get; set; }

        public virtual Invoicer Invoicer { get; set; }
    }
}
