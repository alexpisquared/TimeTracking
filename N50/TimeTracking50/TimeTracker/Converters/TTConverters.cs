using System;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
//using TimeTracker; ++

//xmlns:mvvm="clr-namespace:SBNET.MVVM;assembly=SBNET.MVVM"
//<TextBlock TextAlignment="Center" Text="{Binding Path=Unlading, Converter={mvvm:Equals EqualsText='Y', NotEqualsText='N'}, ConverterParameter=1}" />

namespace AsLink
{
	public class WorkedHrs : MarkupExtension, IValueConverter
	{
		public static double DailyHours { get; set; } = 8;
		public WorkedHrs() { }

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is double)
			{
				return
					((double)value) == 0   /**/ ? new SolidColorBrush(Color.FromRgb(255, 240, 224)) :
					((double)value) < DailyHours  /**/ ? new SolidColorBrush(Color.FromRgb(200, 224, 255)) :
					((double)value) == DailyHours /**/ ? new SolidColorBrush(Color.FromRgb(224, 255, 224)) :
																 /**/   new SolidColorBrush(Color.FromRgb(255, 200, 255));
			}

			return new SolidColorBrush(Colors.Red);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override object ProvideValue(IServiceProvider serviceProvider) { return this; }
	}
}
