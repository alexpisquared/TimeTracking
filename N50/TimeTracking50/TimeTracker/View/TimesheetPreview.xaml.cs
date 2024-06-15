using AsLink;
using System.Windows;
using System.Windows.Input;
using TimeTracker.Properties;

namespace TimeTracker.View
{
  public partial class TimesheetPreview 
  {
    public TimesheetPreview()
    {
      InitializeComponent();
      Closing += TimesheetPreview_Closing;
      DataContext = this;

      if (!string.IsNullOrEmpty(Settings.Default.TShtVw))
      {
        if (Serializer.LoadFromString<AppSettings>(Settings.Default.TShtVw) is AppSettings stgs)
        {
          Top = stgs.Window3.windowTop;
          Left = stgs.Window3.windowLeft;
          Width = stgs.Window3.windowWidth;
          Height = stgs.Window3.windowHeight;
        }
      }
    }

    void TimesheetPreview_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      var stgs = (string.IsNullOrEmpty(Settings.Default.TShtVw) || null == Serializer.LoadFromString<AppSettings>(Settings.Default.TShtVw) as AppSettings) ? new AppSettings() : Serializer.LoadFromString<AppSettings>(Settings.Default.TShtVw) as AppSettings;
      stgs.Window3.windowTop = Top;
      stgs.Window3.windowLeft = Left;
      stgs.Window3.windowWidth = Width;
      stgs.Window3.windowHeight = Height;
      Settings.Default.TShtVw = Serializer.SaveToString(stgs);
      Settings.Default.Save();
    }
  }
}