﻿using AAV.Sys.Ext;
using AAV.Sys.Helpers;
using AAV.WPF.Ext;
using AsLink;
using Db.TimeTrack.DbModel;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TimeTracker.Common;

namespace TimeTracker.View
{
  public partial class InvoicePreview : AAV.WPF.Base.WindowBase
  {
    A0DbContext _db;
    public InvoicePreview(A0DbContext db)
    {
      _db = db;
      InitializeComponent();
      Loaded += onLoaded;
      KeyDown += (s, e) =>
      {
        switch (e.Key)
        {
          case Key.Escape: Close(); break;
          case Key.Left: movePayPrd(-1); break;
          case Key.Right: movePayPrd(+1); break;
        }
      };

      Closing += onClosing;
      DataContext = this;
      AppSettings.RestoreSizePosition(this, Properties.Settings.Default.InvcVw);

      IsVis = Visibility.Collapsed;
      BlurRdus = VerHelper.IsMyHomePC ? 0 : 11;
    }

    void onLoaded(object sender, RoutedEventArgs e)
    {
      var max = _db.Invoices.Max(r => r.Id);
      InvoiceNo = Invoice == null ? max : Invoice.Id > 0 ? Invoice.Id : max + 1;
      if (Invoice == null)
      {
        _db.TimePerDays.Load();
        movePayPrd(0);
      }

      showStatus();
    }
    void onClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      Properties.Settings.Default.InvcVw = AppSettings.SaveSizePosition(this, Properties.Settings.Default.InvcVw); Properties.Settings.Default.Save();

      InfoMsg = FromTillCtgrTaskNote.DbSaveOr(_db, e);
      App.SpeakFaF(InfoMsg);
    }
    async void onSubmit(object sender, RoutedEventArgs e)
    {
      try
      {
        btnInvoiceSubmit.IsEnabled = false;

        InfoMsg = _db.GetDbChangesReport(3);

        if (!Directory.Exists(DSettngS.InvoiceSubFolder))
          DSettngS.InvoiceSubFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"0\Ltd\Invoicing");
        if (!Directory.Exists(DSettngS.InvoiceSubFolder))
          Directory.CreateDirectory(DSettngS.InvoiceSubFolder);

        //1.		Save a hardcopy to a file
        var hardcopyPDF = TimesheetPreviewVM.InvoiceFilepathname(InvoiceNo.ToString(), (double)Invoice.TotalHours, DSettngS.InvoiceSubFolder, "Invoice #", "pdf");
#if XPS
      var hardcopyXPS = TimesheetPreviewVM.InvoiceFilepathname(InvoiceNo.ToString(), (double)Invoice.TotalHours, DSettngS.InvoiceSubFolder, "Invoice #", "xps");
      await revealFor3secondsToGenerateXpsFile(hardcopyXPS);
        new XpsViewer(hardcopyXPS).ShowDialog();
#endif

        //concider : if (VerHelper.IsMyHomePC)
        new InvoiceCreator.PDF.InvoiceMaker().PrepareInvoice(
          DSettngS.Invoicer.CompanyName,
          DSettngS.Invoicer.AddressDetails,
          Invoice.Invoicee.CompanyName,
          Invoice.Invoicee.AddressDetails,
          InvoiceNo,
          $"{PayPrdEnd:d-MMM-yyyy}",
          $"{PayPrdBgn:d-MMM-yyyy}  -  {PayPrdEnd:d-MMM-yyyy}",
          $"{Math.Floor(PayPrdHrs)}:{TimeSpan.FromHours((double)PayPrdHrs):mm}",
          FromTillCtgrTaskNote._svcs,
          $"${Invoice.Invoicee.CorpRate:N0}",
          $"${AmountHR:N2}",
          $"${Subtotal:N2}",
          $"{DSettngS.HstPercent:N0} %",
          $"${SalesTax:N2}",
          $"${GrdTotal:N2}",
          hardcopyPDF);

        var times = generateTimeTrackReport(_db, PayPrdBgn, PayPrdEnd);

        //2a.		Email a letter with the attachment to the current Timesheet approver
        if (!string.IsNullOrEmpty(InvoiceE.TimesheetEmail))
        {
          var bodyTimesheet = string.Format(InvoiceE.TimesheetEmailBody, Invoice.PeriodFrom, Invoice.PeriodUpTo, PayPrdHrs) + times;
          var (exitCode1, errMsg1) = Emailer.PerpAndShow(InvoiceE.TimesheetEmail, $"Alex Pigida – Approval of hours for the period from {Invoice.PeriodFrom:MMMM d, yyyy} through {Invoice.PeriodUpTo:MMMM d, yyyy}. ", bodyTimesheet);
        }

        //2b.		Email a letter with the attachment to the current invoicee
        var bodyInvoice = string.Format(InvoiceE.InvoiceEmailBody, Invoice.PeriodFrom, Invoice.PeriodUpTo, "invoice", "·") + times;
        var (exitCode, errMsg) = Emailer.PerpAndShow(InvoiceE.InvoiceEmail, $"Invoice #{InvoiceNo} for the period {Invoice.PeriodFrom:MMMM d} - {Invoice.PeriodUpTo:MMMM d} ", bodyInvoice, hardcopyPDF); //DayFri.Note += string.Format("\n (timesheet {0} to {1} on {2:MMMd HH:mm})", exitCode == 0 ? "sent" : "sending failed", Invoicee.TimesheetEmail, App.AppStartAt);

        try
        {
          var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"CI\SL");
          Process.Start(new ProcessStartInfo("Explorer.exe", dir));
        }
        catch (Exception ex) { ex.Pop(); }

        var isSuccess = exitCode == 0;

        Invoice.IsSubmitted = isSuccess;
        Invoice.Notes = $"{(isSuccess ? "Sent" : $"sending failed ({errMsg})")} to {Invoice.Invoicee.InvoiceEmail} at {App.AppStartAt}. \n\n\n" + Invoice.Notes;
        if (Invoice.Notes.Length >= 512)
          Invoice.Notes = Invoice.Notes.Substring(0, 512 - 1);

        InfoMsg = await _db.TrySaveReportAsync() + " rows saved";
        App.SpeakFaF(InfoMsg);

        Close();
      }
      catch (Exception ex) { ex.Pop(); }
    }

    string generateTimeTrackReport(A0DbContext db, DateTime payPrdBgn, DateTime payPrdEnd)
    {
      var report = new StringBuilder();
      try
      {
        const string line = "\t\t----------------\t---------";

        report.AppendLine($"───────────────────────────────────────────────────────────────");
        report.AppendLine($"\t\t\t     Timesheet ");
        report.AppendLine($"\t\t       (expediency-copy)");
        report.AppendLine($"\t\t      Date   \t\t Hours");
        report.AppendLine(line);
        var days = db.TimePerDays.Local.Where(r => payPrdBgn <= r.WorkedOn && r.WorkedOn <= payPrdEnd).OrderBy(r => r.WorkedOn);
        var oneIsEnogh = false;
        foreach (var day in days)
        {
          try
          {
            if (day.WorkedHours > 0)
            { report.AppendLine($"\t\t{day.WorkedOn:yyyy-MM-dd}\t{day.WorkedHours,10:N2}"); oneIsEnogh = false; }
            else if (!oneIsEnogh)
            {
              oneIsEnogh = true;
              report.AppendLine("");
            }
          }
          catch (Exception ex) { ex.Log($"TimePerDay.Id:{day.Id}"); }
        }
        report.AppendLine(line);
        report.AppendLine($"\t\t     Total: \t\t{days.Sum(r => r.WorkedHours),7:N2}");
        report.AppendLine($"───────────────────────────────────────────────────────────────");
        report.AppendLine($"                      ..content generated by AAVpro.TimeTracker");
      }
      catch (Exception ex) { ex.Log(); }

      return report.ToString();
    }

    async Task revealFor3secondsToGenerateXpsFile(string hardcopyXPS)
    {
      IsVis = Visibility.Visible;
      //App.SpeakAsync("3 second check..."); await Task.Delay(3000); App.SpeakAsync("...is over.");
      await Task.Delay(30);
      XamlToXps.Export(new Uri(hardcopyXPS, UriKind.Absolute), PrintArea); //review XPS: FixedViewer.Document = new XpsDocument(filename, FileAccess.Read).GetFixedDocumentSequence();
      IsVis = Visibility.Collapsed;
    }

    void onDel(object sender, RoutedEventArgs e)
    {
      foreach (var tpd in _db.TimePerDays.Local.Where(r => r.InvoiceId == Invoice.Id))
        tpd.InvoiceId = null;

      if (_db.Invoices.Any(r => r.Id == Invoice.Id))
        _db.Invoices.Where(r => r.Id == Invoice.Id).ToList().ForEach(dl => _db.Entry(dl).State = EntityState.Deleted);

      showStatus();
    }
    void prevPrd_Click(object sender, RoutedEventArgs e) => movePayPrd(-1);
    void nextPrd_Click(object sender, RoutedEventArgs e) => movePayPrd(+1);     //void btnUnderConstr_Click(object sender, RoutedEventArgs e) { MessageBox.Show("The feature is under contruction\n\nPlease come back soon", ((Button)sender).Content.ToString().Replace("_", "")); }
    void onNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
    {
      try
      {
        var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), e.Uri.OriginalString);
        Process.Start(new ProcessStartInfo(dir)); 
        
        e.Handled = true;
      }
      catch (Exception ex) { ex.Pop(); }
    }

    void movePayPrd(int dd)
    {
      Invoice ivc;
      var idx = InvoiceNo;
      var max = _db.Invoices.Max(r => r.Id);
      do
      {
        idx += dd;
        ivc = _db.Invoices.FirstOrDefault(r => r.Id == idx);
      } while (ivc == null && (idx > 0 && idx < max));

      if (ivc == null)
      {
        Bpr.BeepNo();
        return;
      }

      Invoice = ivc;
      InvoiceNo = idx;
      PayPrdHrs = Invoice.TotalHours;
      PayPrdBgn = Invoice.PeriodFrom;
      PayPrdEnd = Invoice.PeriodUpTo;
      AmountHR = Subtotal = PayPrdHrs * Invoice.RateSubmitted;
      SalesTax = AmountHR * (DSettngS.HstPercent * .01m);
      GrdTotal = AmountHR * (DSettngS.HstPercent * .01m + 1.0m);

      showStatus();
    }

    void showStatus() => InfoMsg = _db.GetDbChangesReport(0) + (Invoice == null ? "\nNull!!!" :
$"\n{(Invoice.IsSubmitted ? "Submitted!!! " : "Not Submitted")} - {_db.Entry(Invoice).State}");

    static void recalcPPH(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var ip = d as InvoicePreview;
      ip.AmountHR = ip.Subtotal = ip.PayPrdHrs * ip.InvoiceE.CorpRate;
      ip.SalesTax = ip.AmountHR * ip.DSettngS.HstPercent * .01m;
      ip.GrdTotal = ip.AmountHR * (ip.DSettngS.HstPercent * .01m + 1m);
    }

    Invoicer _invoiceR;         /**/ public Invoicer InvoiceR { get => _invoiceR ?? (_db.Invoicers.FirstOrDefault(r => r.Id == DSettngS.CurrentInvoicerId)); set => _invoiceR = value; }
    Invoicee _invoiceE;         /**/ public Invoicee InvoiceE { get => _invoiceE ?? (_db.Invoicees.FirstOrDefault(r => r.Id == DSettngS.CurrentInvoiceeId)); set => _invoiceE = value; }
    DefaultSetting _settingS;   /**/ public DefaultSetting DSettngS { get => _settingS ?? (_settingS = _db.DefaultSettings.FirstOrDefault()); set => _settingS = value; }

    //public static readonly DependencyProperty InvoiceeProperty = DependencyProperty.Register("Invoicee", typeof(Invoicee), typeof(InvoicePreview), new PropertyMetadata(null)); public Invoicee Invoicee { get { return (Invoicee)GetValue(InvoiceeProperty); } set { SetValue(InvoiceeProperty, value); } }
    //public static readonly DependencyProperty InvoicerProperty = DependencyProperty.Register("Invoicer", typeof(Invoicer), typeof(InvoicePreview), new PropertyMetadata(null)); public Invoicer Invoicer { get { return (Invoicer)GetValue(InvoicerProperty); } set { SetValue(InvoicerProperty, value); } }
    public static readonly DependencyProperty InvoiceProperty = DependencyProperty.Register("Invoice", typeof(Invoice), typeof(InvoicePreview), new PropertyMetadata(null)); public Invoice Invoice { get => (Invoice)GetValue(InvoiceProperty); set => SetValue(InvoiceProperty, value); }
    public static readonly DependencyProperty PayPrdBgnProperty = DependencyProperty.Register("PayPrdBgn", typeof(DateTime), typeof(InvoicePreview), new PropertyMetadata(DateTime.MinValue)); public DateTime PayPrdBgn { get => (DateTime)GetValue(PayPrdBgnProperty); set => SetValue(PayPrdBgnProperty, value); }
    public static readonly DependencyProperty PayPrdEndProperty = DependencyProperty.Register("PayPrdEnd", typeof(DateTime), typeof(InvoicePreview), new PropertyMetadata(DateTime.MinValue)); public DateTime PayPrdEnd { get => (DateTime)GetValue(PayPrdEndProperty); set => SetValue(PayPrdEndProperty, value); }
    public static readonly DependencyProperty PayPrdHrsProperty = DependencyProperty.Register("PayPrdHrs", typeof(decimal), typeof(InvoicePreview), new PropertyMetadata(0.0m, recalcPPH)); public decimal PayPrdHrs { get => (decimal)GetValue(PayPrdHrsProperty); set => SetValue(PayPrdHrsProperty, value); }
    public static readonly DependencyProperty InvoiceNoProperty = DependencyProperty.Register("InvoiceNo", typeof(int), typeof(InvoicePreview), new PropertyMetadata(97738)); public int InvoiceNo { get => (int)GetValue(InvoiceNoProperty); set => SetValue(InvoiceNoProperty, value); }
    public static readonly DependencyProperty AmountHRProperty = DependencyProperty.Register("AmountHR", typeof(decimal), typeof(InvoicePreview), new PropertyMetadata(123.456m)); public decimal AmountHR { get => (decimal)GetValue(AmountHRProperty); set => SetValue(AmountHRProperty, value); }
    public static readonly DependencyProperty SubtotalProperty = DependencyProperty.Register("Subtotal", typeof(decimal), typeof(InvoicePreview), new PropertyMetadata(123.456m)); public decimal Subtotal { get => (decimal)GetValue(SubtotalProperty); set => SetValue(SubtotalProperty, value); }
    public static readonly DependencyProperty SalesTaxProperty = DependencyProperty.Register("SalesTax", typeof(decimal), typeof(InvoicePreview), new PropertyMetadata(123.456m)); public decimal SalesTax { get => (decimal)GetValue(SalesTaxProperty); set => SetValue(SalesTaxProperty, value); }
    public static readonly DependencyProperty GrdTotalProperty = DependencyProperty.Register("GrdTotal", typeof(decimal), typeof(InvoicePreview), new PropertyMetadata(123.456m)); public decimal GrdTotal { get => (decimal)GetValue(GrdTotalProperty); set => SetValue(GrdTotalProperty, value); }
    public static readonly DependencyProperty InfoMsgProperty = DependencyProperty.Register("InfoMsg", typeof(string), typeof(InvoicePreview), new PropertyMetadata(null)); public string InfoMsg { get => (string)GetValue(InfoMsgProperty); set => SetValue(InfoMsgProperty, value); }
    public static readonly DependencyProperty BlurRdusProperty = DependencyProperty.Register("BlurRdus", typeof(decimal), typeof(InvoicePreview), new PropertyMetadata(123.456m)); public decimal BlurRdus { get => (decimal)GetValue(BlurRdusProperty); set => SetValue(BlurRdusProperty, value); }
    public static readonly DependencyProperty IsVisProperty = DependencyProperty.Register("IsVis", typeof(Visibility), typeof(InvoicePreview), new PropertyMetadata()); public Visibility IsVis { get => (Visibility)GetValue(IsVisProperty); set => SetValue(IsVisProperty, value); }
  }
}
