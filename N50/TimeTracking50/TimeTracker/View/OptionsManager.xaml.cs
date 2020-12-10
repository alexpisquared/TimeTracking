using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using AsLink;
using TimeTracker.Properties;
using System.Windows.Controls;

namespace TimeTracker.View
{
  public partial class OptionsManager : AAV.WPF.Base.WindowBase
  {
    public OptionsManager()
    {
      InitializeComponent();
      KeyDown += (s, e) => { if (e.Key == Key.Escape) { Close(); } };
      Closing += Window_Closing;
      DataContext = this;

      if (!string.IsNullOrEmpty(Settings.Default.OptnVw))
      {
        var stgs = Serializer.LoadFromString<AppSettings>(Settings.Default.OptnVw) as AppSettings;
        if (stgs.Window2 != null)
        {
          Top = stgs.Window2.windowTop;
          Left = stgs.Window2.windowLeft;
          //Width = stgs.Window2.windowWidth;
          //Height = stgs.Window2.windowHeight;
        }
      }
    }

    Db.TimeTrack.DbModel.A0DbContext ctx = Db.TimeTrack.DbModel.A0DbContext.Create();

    public string InfoMsg { get { return (string)GetValue(InfoMsgProperty); } set { SetValue(InfoMsgProperty, value); } }
    public static readonly DependencyProperty InfoMsgProperty = DependencyProperty.Register("InfoMsg", typeof(string), typeof(OptionsManager), new PropertyMetadata(null));

    void dataCorrection()
    {
      //var src = ((CollectionViewSource)(this.FindResource("timeIntervalViewSource")));
      //if (src == null || src.View == null)
      //foreach (var r in ((System.Windows.Data.CollectionView)(timeDg1.DataContext)).SourceCollection) rowCorrection(r);
      //else
      //	foreach (var r in src.View) rowCorrection(r);
    }
    void correctAndSaveToDb()
    {
      try { dataCorrection(); InfoMsg = $"{ctx.SaveChanges()} rows saved"; }
      catch (DbEntityValidationException ex)
      {
        foreach (var er in ex.EntityValidationErrors)
        {
          Debug.WriteLine(er.Entry.Entity.GetType(), "\t");
          foreach (var ve in er.ValidationErrors)
          {
            Debug.WriteLine(ve.ErrorMessage, "\t\t");
            MessageBox.Show(ve.ErrorMessage);
          }
        }
      }
      catch (InvalidOperationException ex) { MessageBox.Show(ex.ToString(), "InvalidOperationException has been thrown", MessageBoxButton.OK, MessageBoxImage.Error); }
      catch (Exception ex) { if (Debugger.IsAttached) Debugger.Break(); MessageBox.Show(ex.ToString(), "Exception has been thrown", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ctx.DefaultSettings.Load();
      ctx.Invoicers.Load();
      ctx.Invoicees.Load();
      ctx.lkuJobCategories.Load();
      ctx.lkuPayPeriodModes.Load();

      ((CollectionViewSource)(this.FindResource("defaultSettingViewSource"))).Source = ctx.DefaultSettings.Local;
      ((CollectionViewSource)(this.FindResource("invoicerViewSource"))).Source = ctx.Invoicers.Local;
      ((CollectionViewSource)(this.FindResource("invoiceeViewSource"))).Source = ctx.Invoicees.Local;
      ((CollectionViewSource)(this.FindResource("lkuJobCategoryViewSource"))).Source = ctx.lkuJobCategories.Local;
      ((CollectionViewSource)(this.FindResource("lkuPayPeriodModeViewSource"))).Source = ctx.lkuPayPeriodModes.Local;
    }
    void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e0)
    {
      try
      {
        if (ctx.ChangeTracker.Entries().Any(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
        {
          var question = "Would you like to save the changes?";
          var header = "Changes detected";
          switch (MessageBox.Show(question, header, MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
          {
            case MessageBoxResult.Yes: correctAndSaveToDb(); break;
            case MessageBoxResult.No: break;
            case MessageBoxResult.Cancel: e0.Cancel = true; break;
          }
        }
      }
      catch (Exception ex) { MessageBox.Show(ex.ToString()); }

      var stgs = (string.IsNullOrEmpty(Settings.Default.OptnVw) || null == Serializer.LoadFromString<AppSettings>(Settings.Default.OptnVw) as AppSettings) ? new AppSettings() : Serializer.LoadFromString<AppSettings>(Settings.Default.OptnVw) as AppSettings;
      stgs.Window2.windowTop = Top;
      stgs.Window2.windowLeft = Left;
      stgs.Window2.windowWidth = Width;
      stgs.Window2.windowHeight = Height;
      Settings.Default.OptnVw = Serializer.SaveToString(stgs);
      Settings.Default.Save();

      new FromTillCtgrTaskNote().Show();
    }
    void btnSave_Click(object sender, RoutedEventArgs e) { correctAndSaveToDb(); Close(); }
    void btnQuit_Click(object sender, RoutedEventArgs e) { Close(); }

    void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { App.SpeakAsync("Do not forget to adjust the pay period length."); }
  }


  public static class PasswordHelper
  {
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper), new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));
    public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));
    static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordHelper));

    public static void SetAttach(DependencyObject dp, bool value)
    {
      dp.SetValue(AttachProperty, value);
    }
    public static bool GetAttach(DependencyObject dp)
    {
      return (bool)dp.GetValue(AttachProperty);
    }
    public static string GetPassword(DependencyObject dp)
    {
      return (string)dp.GetValue(PasswordProperty);
    }
    public static void SetPassword(DependencyObject dp, string value)
    {
      dp.SetValue(PasswordProperty, value);
    }
    static bool GetIsUpdating(DependencyObject dp)
    {
      return (bool)dp.GetValue(IsUpdatingProperty);
    }
    static void SetIsUpdating(DependencyObject dp, bool value)
    {
      dp.SetValue(IsUpdatingProperty, value);
    }

    static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      PasswordBox passwordBox = sender as PasswordBox;
      passwordBox.PasswordChanged -= PasswordChanged;

      if (!(bool)GetIsUpdating(passwordBox))
      {
        passwordBox.Password = (string)e.NewValue;
      }
      passwordBox.PasswordChanged += PasswordChanged;
    }
    static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (!(sender is PasswordBox passwordBox))
        return;

      if ((bool)e.OldValue)
      {
        passwordBox.PasswordChanged -= PasswordChanged;
      }

      if ((bool)e.NewValue)
      {
        passwordBox.PasswordChanged += PasswordChanged;
      }
    }

    static void PasswordChanged(object sender, RoutedEventArgs e)
    {
      PasswordBox passwordBox = sender as PasswordBox;
      SetIsUpdating(passwordBox, true);
      SetPassword(passwordBox, passwordBox.Password);
      SetIsUpdating(passwordBox, false);
    }
  }
}
