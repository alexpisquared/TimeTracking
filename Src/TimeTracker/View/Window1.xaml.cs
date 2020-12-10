using System.Windows;

namespace TimeTracker.View
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : AAV.WPF.Base.WindowBase
	{
		public Window1()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{

			System.Windows.Data.CollectionViewSource timePerDayViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("timePerDayViewSource")));
			// Load data by setting the CollectionViewSource.Source property:
			// timePerDayViewSource.Source = [generic data source]
		}
	}
}
