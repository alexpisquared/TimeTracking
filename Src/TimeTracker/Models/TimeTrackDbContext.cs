using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using TimeTracker.Models.Mapping;

namespace TimeTracker.Models
{
	public partial class TimeTrackDbContext : DbContext
	{
		static TimeTrackDbContext()
		{
			Database.SetInitializer<TimeTrackDbContext>(null);
		}

		public TimeTrackDbContext()
#if DEBUG // if DEBUG - take the default DB: TimeTracker.Models.TimeTrackDbContext
			: base("Name=TimeTrackDbCntxDbg")
#else
            : base("Name=TimeTrackDbCntxRls")
#endif
		{
		}

		public DbSet<DefaultSetting> DefaultSettings { get; set; }
		public DbSet<Invoice> Invoices { get; set; }
		public DbSet<Invoicee> Invoicees { get; set; }
		public DbSet<Invoicer> Invoicers { get; set; }
		public DbSet<lkuJobCategory> lkuJobCategories { get; set; }
		public DbSet<lkuPayPeriodMode> lkuPayPeriodModes { get; set; }
		//public DbSet<lkuTaskStatu> lkuTaskStatus { get; set; }
		//public DbSet<Person> People { get; set; }
		//public DbSet<sysdiagram> sysdiagrams { get; set; }
		//public DbSet<TaskCategory> TaskCategories { get; set; }
		//public DbSet<TaskItem> TaskItems { get; set; }
		//public DbSet<TimeInterval> TimeIntervals { get; set; }
		public DbSet<TimePerDay> TimePerDays { get; set; }
		//public DbSet<Track1Simple> Track1Simple { get; set; }
		//public DbSet<Track2FromTo> Track2FromTo { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new DefaultSettingMap());
			modelBuilder.Configurations.Add(new InvoiceMap());
			modelBuilder.Configurations.Add(new InvoiceeMap());
			modelBuilder.Configurations.Add(new InvoicerMap());
			modelBuilder.Configurations.Add(new lkuJobCategoryMap());
			modelBuilder.Configurations.Add(new lkuPayPeriodModeMap());
			//modelBuilder.Configurations.Add(new lkuTaskStatuMap());
			//modelBuilder.Configurations.Add(new PersonMap());
			//modelBuilder.Configurations.Add(new sysdiagramMap());
			//modelBuilder.Configurations.Add(new TaskCategoryMap());
			//modelBuilder.Configurations.Add(new TaskItemMap());
			//modelBuilder.Configurations.Add(new TimeIntervalMap());
			modelBuilder.Configurations.Add(new TimePerDayMap());
			//modelBuilder.Configurations.Add(new Track1SimpleMap());
			//modelBuilder.Configurations.Add(new Track2FromToMap());
		}
	}
}
