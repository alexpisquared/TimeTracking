namespace TimeTracker.AsLink;
public class TimeTrackDbCtx_Code1st_DbInitializer : DropCreateDatabaseIfModelChanges<A0DbContext>
{
  public static void DbIni()
  {
    try
    {
      DropCreateDb();

      var db = A0DbContext.Create();

      db.lkuJobCategories.Load();
      foreach (var item in db.lkuJobCategories.Local) Debug.WriteLine(item.Name);
      foreach (var item in db.lkuJobCategories) Debug.WriteLine(item.Name);
      //foreach (var item in db.DefaultSettings) Debug.WriteLine(item); db.DefaultSettings.Load(); foreach (var item in db.DefaultSettings.Local) Debug.WriteLine(item);
    }
    catch (Exception ex) { SystemSounds.Exclamation.Play(); Console.WriteLine(ex); }
  }
  public static void DropCreateDb()
  {
    try
    {
      Database.SetInitializer(new CreateDatabaseIfNotExists<A0DbContext>());
      Database.SetInitializer(new DropCreateDatabaseIfModelChanges<A0DbContext>());
      Database.SetInitializer(new TimeTrackDbCtx_Code1st_DbInitializer());
    }
    catch (Exception ex) { _ = MessageBox.Show(ex.ToString(), "InvalidOperationException has been thrown", MessageBoxButton.OK, MessageBoxImage.Error); }
  }

  protected override void Seed(A0DbContext x)
  {
    try
    {
      base.Seed(x);

      _ = x.lkuJobCategories.Add(new lkuJobCategory { Id = "dev", Name = "Development", Description = "Software Engineering Services" });
      _ = x.lkuJobCategories.Add(new lkuJobCategory { Id = "mtg", Name = "Meetings", Description = "Meetings" });
      _ = x.lkuJobCategories.Add(new lkuJobCategory { Id = "off", Name = "Time off", Description = "Time off" });
      _ = x.lkuJobCategories.Add(new lkuJobCategory { Id = "vac", Name = "Vacation", Description = "Vacation" });

      //				x.lkuPayPeriodModes.Add(new lkuPayPeriodMode { Id = "wk1", Name = "Weekly", Description = "" });
      //				x.lkuPayPeriodModes.Add(new lkuPayPeriodMode { Id = "wk2", Name = "ByWeekly", Description = "" });
      //				x.lkuPayPeriodModes.Add(new lkuPayPeriodMode { Id = "m15", Name = "Semi-Monthly", Description = "" });
      //				x.lkuPayPeriodModes.Add(new lkuPayPeriodMode { Id = "mon", Name = "Monthly", Description = "" });

      //				x.Invoicees.Add(new Invoicee
      //				{
      //					CompanyName = "Hays Specialist Recruitment (Canada) Inc.",
      //					AddressDetails =
      //@"c/o Accounts Payable
      //402 - 1500 Don Mills Rd
      //North York ON M3B 3K4",
      //					InvoiceEmail = "tester@livingstonintl.com",
      //					InvoiceEmailBody =
      //@"Dear Sirs,  
      //Attached please find the invoice for the period from {0:MMMM d, yyyy} through {1:MMMM d, yyyy}.
      //Best regards, 
      //				 
      //John Smith",
      //					CorpRate = 1000,
      //					StartDate = new DateTime(2013, 6, 30),
      //					PayPeriodMode = "wk1",
      //					PayPeriodStart = 0,
      //					PayPeriodLength = 7,
      //					HoursPerPeriod = 40.0m,
      //					WebUsername = "gole00",
      //					WebPassword = "gole00",
      //					Notes = "Nice agency to work with."
      //				});
      //				x.Invoicees.Add(new Invoicee
      //				{
      //					CompanyName = "Microsoft Canada",
      //					AddressDetails = @"multiline address
      //for invoicing
      //goes here",
      //					InvoiceEmail = "tester@microsoft.com",
      //					InvoiceEmailBody = @"Dear Sirs,  
      //Attached please find the invoice for the period from {0:MMMM d, yyyy} through {1:MMMM d, yyyy}.
      //Best regards, 
      // 
      //John Smith",
      //					CorpRate = 1000,
      //					StartDate = new DateTime(2013, 6, 30),
      //					PayPeriodMode = "wk1",
      //					PayPeriodStart = 0,
      //					PayPeriodLength = 7,
      //					HoursPerPeriod = 40.0m,
      //					Notes = "Nicer company to work for."
      //				});
      //				x.SaveChanges();
      //				var curInvoicee = x.Invoicees.Add(new Invoicee
      //				{
      //					CompanyName = "BMO",
      //					AddressDetails = @"multiline address
      //for invoicing
      //goes here",
      //					InvoiceEmail = "tester@live.com",
      //					InvoiceEmailBody = @"Dear Sirs,  
      //Attached please find the invoice for the period from {0:MMMM d, yyyy} through {1:MMMM d, yyyy}.
      //Best regards, 
      // 
      //John Smith",
      //					CorpRate = 1000,
      //					StartDate = DateTime.Now.AddDays(7),
      //					PayPeriodMode = "wk1",
      //					PayPeriodStart = 0,
      //					PayPeriodLength = 7,
      //					HoursPerPeriod = 40.0m,
      //					Notes = "Nicest company to work for."
      //				});

      //				var curInvoicer = x.Invoicers.Add(new Invoicer
      //				{
      //					CompanyName = "My Company Name",
      //					AddressDetails = @"(1234567 Ontario Inc)
      //111 King Street West
      //Toronto, ON M3M 3M3
      //Phone (416) 123-4567
      //GST # 123456789RT0001
      //John Smith ",
      //					InvoiceEmail = "tester@livingstonintl.com"
      //				});

      //				x.SaveChanges();

      //				x.DefaultSettings.Add(new DefaultSetting { Invoicee = curInvoicee, Invoicer = curInvoicer, HstPercent = 13m, DayStartHour = 8.5, LunchStartHour = 12.5, DefaultJobCategoryId = "dev", InvoiceSubFolder = "Invoices", CreatedBy = Environment.UserDomainName + @"\" + Environment.UserName, CreatedAt = DateTime.Now });

      _ = x.SaveChanges();
      //			}
      //			catch (DbEntityValidationException ex)
      //			{
      //				foreach (var er in ex.EntityValidationErrors)
      //				{
      //					Debug.WriteLine(er.Entry.Entity.GetType(), "\t");
      //					foreach (var ve in er.ValidationErrors)
      //					{
      //						Debug.WriteLine(ve.ErrorMessage, "\t\t");
      //						MessageBox.Show(ve.ErrorMessage);
      //					}
      //				}
    }
    catch (Exception ex) { _ = MessageBox.Show(ex.ToString(), "InvalidOperationException has been thrown", MessageBoxButton.OK, MessageBoxImage.Error); }
  }
}
