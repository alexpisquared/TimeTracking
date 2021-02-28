namespace Db.TimeTrack.DbModel
{
	using System.Data.Entity;

	public partial class A0DbContext : DbContext
  {
    //public A0DbContext()
    //    : base("name=A0DbContext")
    //{
    //}

    public virtual DbSet<DefaultSetting> DefaultSettings { get; set; }
    public virtual DbSet<Invoice> Invoices { get; set; }
    public virtual DbSet<Invoicee> Invoicees { get; set; }
    public virtual DbSet<Invoicer> Invoicers { get; set; }
    public virtual DbSet<lkuJobCategory> lkuJobCategories { get; set; }
    public virtual DbSet<lkuPayPeriodMode> lkuPayPeriodModes { get; set; }
    public virtual DbSet<lkuTaskStatu> lkuTaskStatus { get; set; }
    public virtual DbSet<Person> People { get; set; }
    public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    public virtual DbSet<TaskCategory> TaskCategories { get; set; }
    public virtual DbSet<TaskItem> TaskItems { get; set; }
    public virtual DbSet<TimeInterval> TimeIntervals { get; set; }
    public virtual DbSet<TimePerDay> TimePerDays { get; set; }
    public virtual DbSet<Track1Simple> Track1Simple { get; set; }
    public virtual DbSet<Track2FromTo> Track2FromTo { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<DefaultSetting>()
          .Property(e => e.HstPercent)
          .HasPrecision(19, 4);

      modelBuilder.Entity<DefaultSetting>()
          .Property(e => e.XmlSomething)
          .IsUnicode(false);

      modelBuilder.Entity<DefaultSetting>()
          .Property(e => e.Notes)
          .IsUnicode(false);

      modelBuilder.Entity<DefaultSetting>()
          .Property(e => e.CreatedBy)
          .IsUnicode(false);

      modelBuilder.Entity<DefaultSetting>()
          .Property(e => e.DefaultJobCategoryId)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<DefaultSetting>()
          .Property(e => e.InvoiceSubFolder)
          .IsUnicode(false);

      modelBuilder.Entity<Invoice>()
          .Property(e => e.TotalHours)
          .HasPrecision(18, 0);

      modelBuilder.Entity<Invoice>()
          .Property(e => e.HstPercent)
          .HasPrecision(18, 0);

      modelBuilder.Entity<Invoice>()
          .Property(e => e.RateSubmitted)
          .HasPrecision(19, 4);

      modelBuilder.Entity<Invoice>()
          .Property(e => e.Notes)
          .IsUnicode(false);

      modelBuilder.Entity<Invoicee>()
          .Property(e => e.CompanyName)
          .IsUnicode(false);

      modelBuilder.Entity<Invoicee>()
          .Property(e => e.AddressDetails)
          .IsUnicode(false);

      modelBuilder.Entity<Invoicee>()
          .Property(e => e.InvoiceEmail)
          .IsUnicode(false);

      modelBuilder.Entity<Invoicee>()
          .Property(e => e.InvoiceEmailBody)
          .IsUnicode(false);

      modelBuilder.Entity<Invoicee>()
          .Property(e => e.CorpRate)
          .HasPrecision(19, 4);

      modelBuilder.Entity<Invoicee>()
          .Property(e => e.PayPeriodMode)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<Invoicee>()
          .Property(e => e.Notes)
          .IsUnicode(false);

      modelBuilder.Entity<Invoicee>()
          .HasMany(e => e.DefaultSettings)
          .WithRequired(e => e.Invoicee)
          .HasForeignKey(e => e.CurrentInvoiceeId)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<Invoicer>()
          .Property(e => e.CompanyName)
          .IsUnicode(false);

      modelBuilder.Entity<Invoicer>()
          .Property(e => e.AddressDetails)
          .IsUnicode(false);

      modelBuilder.Entity<Invoicer>()
          .Property(e => e.FromEmail)
          .IsUnicode(false);

      modelBuilder.Entity<Invoicer>()
          .HasMany(e => e.DefaultSettings)
          .WithRequired(e => e.Invoicer)
          .HasForeignKey(e => e.CurrentInvoicerId)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<lkuJobCategory>()
          .Property(e => e.Id)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<lkuJobCategory>()
          .Property(e => e.Name)
          .IsUnicode(false);

      modelBuilder.Entity<lkuJobCategory>()
          .Property(e => e.Description)
          .IsUnicode(false);

      modelBuilder.Entity<lkuJobCategory>()
          .HasMany(e => e.TimeIntervals)
          .WithOptional(e => e.lkuJobCategory)
          .HasForeignKey(e => e.JobCategoryId);

      modelBuilder.Entity<lkuJobCategory>()
          .HasMany(e => e.TimePerDays)
          .WithOptional(e => e.lkuJobCategory)
          .HasForeignKey(e => e.JobCategoryId);

      modelBuilder.Entity<lkuPayPeriodMode>()
          .Property(e => e.Id)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<lkuPayPeriodMode>()
          .Property(e => e.Name)
          .IsUnicode(false);

      modelBuilder.Entity<lkuPayPeriodMode>()
          .Property(e => e.Description)
          .IsUnicode(false);

      modelBuilder.Entity<lkuPayPeriodMode>()
          .HasMany(e => e.Invoicees)
          .WithRequired(e => e.lkuPayPeriodMode)
          .HasForeignKey(e => e.PayPeriodMode)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<lkuTaskStatu>()
          .Property(e => e.Id)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<lkuTaskStatu>()
          .Property(e => e.Name)
          .IsUnicode(false);

      modelBuilder.Entity<lkuTaskStatu>()
          .Property(e => e.Description)
          .IsUnicode(false);

      modelBuilder.Entity<lkuTaskStatu>()
          .HasMany(e => e.TaskItems)
          .WithRequired(e => e.lkuTaskStatu)
          .HasForeignKey(e => e.TaskStatusId)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<Person>()
          .Property(e => e.NameShort)
          .IsUnicode(false);

      modelBuilder.Entity<Person>()
          .Property(e => e.NameFull)
          .IsUnicode(false);

      modelBuilder.Entity<Person>()
          .Property(e => e.Email)
          .IsUnicode(false);

      modelBuilder.Entity<Person>()
          .Property(e => e.Notes)
          .IsUnicode(false);

      modelBuilder.Entity<Person>()
          .HasMany(e => e.TaskItems)
          .WithOptional(e => e.Person)
          .HasForeignKey(e => e.CreatedBy);

      modelBuilder.Entity<Person>()
          .HasMany(e => e.TaskItems1)
          .WithOptional(e => e.Person1)
          .HasForeignKey(e => e.AsignedTo);

      modelBuilder.Entity<Person>()
          .HasMany(e => e.TaskItems2)
          .WithOptional(e => e.Person2)
          .HasForeignKey(e => e.AssignedBy);

      modelBuilder.Entity<Person>()
          .HasMany(e => e.TaskItems3)
          .WithOptional(e => e.Person3)
          .HasForeignKey(e => e.CompleteBy);

      modelBuilder.Entity<Person>()
          .HasMany(e => e.TimeIntervals)
          .WithOptional(e => e.Person)
          .HasForeignKey(e => e.DoneById);

      modelBuilder.Entity<TaskCategory>()
          .Property(e => e.Name)
          .IsUnicode(false);

      modelBuilder.Entity<TaskCategory>()
          .Property(e => e.Description)
          .IsUnicode(false);

      modelBuilder.Entity<TaskCategory>()
          .Property(e => e.Notes)
          .IsUnicode(false);

      modelBuilder.Entity<TaskCategory>()
          .Property(e => e.CreatedBy)
          .IsUnicode(false);

      modelBuilder.Entity<TaskItem>()
          .Property(e => e.Name)
          .IsUnicode(false);

      modelBuilder.Entity<TaskItem>()
          .Property(e => e.Description)
          .IsUnicode(false);

      modelBuilder.Entity<TaskItem>()
          .Property(e => e.TaskStatusId)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<TimeInterval>()
          .Property(e => e.JobCategoryId)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<TimeInterval>()
          .Property(e => e.Note)
          .IsUnicode(false);

      modelBuilder.Entity<TimePerDay>()
          .Property(e => e.JobCategoryId)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<TimePerDay>()
          .Property(e => e.Note)
          .IsUnicode(false);

      modelBuilder.Entity<Track1Simple>()
          .Property(e => e.Username)
          .IsUnicode(false);

      modelBuilder.Entity<Track1Simple>()
          .Property(e => e.Description)
          .IsUnicode(false);

      modelBuilder.Entity<Track2FromTo>()
          .Property(e => e.Username)
          .IsUnicode(false);

      modelBuilder.Entity<Track2FromTo>()
          .Property(e => e.Description)
          .IsUnicode(false);
    }
  }
}
