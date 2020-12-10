using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace MediaQA.MVVM2.Models
{
	public partial class MediaInfo
	{
		[Key]
		public string File { get; set; }
		public System.DateTime Time { get; set; }
		//public int Eval { get; set; }
		public Nullable<System.DateTime> EvalAt { get; set; }
		public bool NotFound { get; set; }
		public int BurnedTimes { get; set; }
	}

	public class TimeTrackDbCtx_Simple : DbContext
	{
		public DbSet<MediaInfo> MediaInfos { get; set; }
		void foo() { }
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
		}
	}
}
