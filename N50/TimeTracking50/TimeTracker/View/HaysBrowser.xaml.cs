using mshtml;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Db.TimeTrack.DbModel;

namespace TimeTracker.View
{
	/// <summary>
	/// Interaction logic for HaysBrowser.xaml
	/// </summary>
	public partial class HaysBrowser : AAV.WPF.Base.WindowBase
	{
		public HaysBrowser()
		{
			InitializeComponent();
			KeyDown += (s, e) => { if (e.Key == Key.Escape) { Close(); } };
		}

		DefaultSetting _settings;

		public DefaultSetting Settings { get { return _settings; } set { _settings = value; } }

		private void login()
		{
			var d = (HTMLDocument)((WebBrowser)(wb1)).Document; //dynamic d = wb1.Document;

			d.getElementById("ASPxRoundPanel3_loginControl_m_UserName").innerText = _settings.Invoicee.WebUsername;
			d.getElementById("ASPxRoundPanel3_loginControl_m_Password").innerText = _settings.Invoicee.WebPassword;

			object obj = d.getElementById("ASPxRoundPanel3_loginControl_btnLogin");
			obj.GetType().GetMethod("click").Invoke(obj, new object[0]);

			b1.IsEnabled = false;

			//var firstMatchingSubmit = (from input in d.getElementsByTagName("input")
			//													 where input.GetAttribute("type") == "submit" && input.GetAttribute("value") == "Sign out"
			//													 select input).FirstOrDefault();
			//if (firstMatchingSubmit != null)
			//	firstMatchingSubmit.RaiseEvent("click");

			//d.forms.item.InvokeMember("submit");
		}

		private void btnLogin_Click(object sender, RoutedEventArgs e) { login(); }
		private void wb1_Navigated(object sender, NavigationEventArgs e)
		{
			Task.Factory.StartNew(() => Thread.Sleep(999)).ContinueWith(_ =>
			{
				if (b1.IsEnabled == true)
					login();
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}
		private void b1_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
