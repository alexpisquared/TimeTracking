using Db.TimeTrack.DbModel;
//using mshtml;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace TimeTracker.View
{
  public partial class HaysBrowser // : AAV.WPF.Base.WindowBase
  {
    public HaysBrowser()
    {
      InitializeComponent();
      KeyDown += (s, e) => { if (e.Key == Key.Escape) { Close(); } };
    }

    DefaultSetting _settings;

    public DefaultSetting Settings { get => _settings; set => _settings = value; }

    void login()
    {
      var d = wb1.Document; //dynamic d = wb1.Document;

      //d.getElementById("ASPxRoundPanel3_loginControl_m_UserName").innerText = _settings.Invoicee.WebUsername;
      //d.getElementById("ASPxRoundPanel3_loginControl_m_Password").innerText = _settings.Invoicee.WebPassword;

      //object obj = d.getElementById("ASPxRoundPanel3_loginControl_btnLogin");
      //obj.GetType().GetMethod("click").Invoke(obj, new object[0]);

      //b1.IsEnabled = false;

      ////var firstMatchingSubmit = (from input in d.getElementsByTagName("input")
      ////													 where input.GetAttribute("type") == "submit" && input.GetAttribute("value") == "Sign out"
      ////													 select input).FirstOrDefault();
      ////if (firstMatchingSubmit != null)
      ////	firstMatchingSubmit.RaiseEvent("click");

      ////d.forms.item.InvokeMember("submit");
    }

    void btnLogin_Click(object sender, RoutedEventArgs e) => login();
    void wb1_Navigated(object sender, NavigationEventArgs e) => Task.Factory.StartNew(() => Thread.Sleep(999)).ContinueWith(_ =>
                                                              {
                                                                if (b1.IsEnabled == true)
                                                                  login();
                                                              }, TaskScheduler.FromCurrentSynchronizationContext());
    void b1_Click(object sender, RoutedEventArgs e) { }
  }
}
