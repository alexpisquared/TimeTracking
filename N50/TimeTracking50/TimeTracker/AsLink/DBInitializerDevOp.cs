using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Media;

namespace MediaQA.MVVM2.Models
{
	public class TimeTrackDbCtx_Simple__DbInitializer : DropCreateDatabaseIfModelChanges<TimeTrackDbCtx_Simple>
	{
		public static void DbIni()
		{
			try
			{
				DropCreateDb();

				var db = new TimeTrackDbCtx_Simple();

				db.MediaInfos.Load(); foreach (var mi in db.MediaInfos.Local) Debug.WriteLine(mi.File);
			}
			catch (Exception ex) { SystemSounds.Exclamation.Play(); Console.WriteLine(ex); }
		}
		public static void DropCreateDb()
		{
			try
			{
				Database.SetInitializer(new CreateDatabaseIfNotExists<TimeTrackDbCtx_Simple>());
				Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TimeTrackDbCtx_Simple>());
				Database.SetInitializer(new TimeTrackDbCtx_Simple__DbInitializer());
			}
			catch (Exception ex) { SystemSounds.Exclamation.Play(); Console.WriteLine(ex); }
		}
		protected override void Seed(TimeTrackDbCtx_Simple db)
		{
			try
			{
				base.Seed(db);

				db.MediaInfos.Add(new MediaInfo { File = @"C:\1\M\rp\NotEvaluated.wma", Time = DateTime.Now });
				db.MediaInfos.Add(new MediaInfo { File = @"C:\1\M\rp\EvalMissed__.wma", Time = DateTime.Now, EvalAt = DateTime.Now });
				db.MediaInfos.Add(new MediaInfo { File = @"C:\1\M\rp\EvalGarbage_.wma", Time = DateTime.Now, EvalAt = DateTime.Now });
				db.SaveChanges();
			}
			catch (Exception ex) { SystemSounds.Exclamation.Play(); Console.WriteLine(ex); }
		}
	}
}
