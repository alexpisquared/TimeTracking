namespace Db.TimeTrack.DbModel
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("Invoicee")]
  public partial class Invoicee
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Invoicee()    {      DefaultSettings = new HashSet<DefaultSetting>();      Invoices = new HashSet<Invoice>();    }
    public int Id { get; set; }
    [Required] [StringLength(150)] public string CompanyName { get; set; }
    [Required] [StringLength(500)] public string AddressDetails { get; set; }
    [Required] [StringLength(150)] public string InvoiceEmail { get; set; }
    [StringLength(150)] public string TimesheetEmail { get; set; }
    [StringLength(150)] public string TimesheetManager { get; set; }
    [StringLength(150)] public string TimesheetCompany { get; set; }
    [Required] [StringLength(2000)] public string InvoiceEmailBody { get; set; }
    [StringLength(2000)] public string TimesheetEmailBody { get; set; }
    [StringLength(150)] public string TimesheetEmailSubj { get; set; }
    [StringLength(150)] public string InvoiceEmailSubj { get; set; }
    [Column(TypeName = "money")] public decimal CorpRate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? LasttDate { get; set; }
    [Required] [StringLength(3)] public string PayPeriodMode { get; set; }
    public int PayPeriodStart { get; set; }
    public int PayPeriodLength { get; set; }
    public decimal HoursPerPeriod { get; set; }
    [StringLength(150)] public string WebUsername { get; set; }
    [StringLength(150)] public string WebPassword { get; set; }
    [Required] public string Notes { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<DefaultSetting> DefaultSettings { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<Invoice> Invoices { get; set; }
    public virtual lkuPayPeriodMode lkuPayPeriodMode { get; set; }
  }
}
