using AsLink;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Xps.Packaging;

namespace TimeTracker.View
{
	public partial class XpsViewer : AAV.WPF.Base.WindowBase
	{
		public XpsViewer(string docFile)
		{
			InitializeComponent();

			KeyDown += (s, e) =>
			{
				switch (e.Key)
				{
					case Key.Escape: Close(); break;
				}
			};

			Loaded += (s, e) =>
			{
				var doc = new XpsDocument(docFile, FileAccess.Read);
				dv1.Document = doc.GetFixedDocumentSequence();
				doc.Close();
			};
			AppSettings.RestoreSizePosition(this, Properties.Settings.Default.XpsVw);
		}

		protected override void OnClosed(EventArgs e)
		{
			Properties.Settings.Default.XpsVw = AppSettings.SaveSizePosition(this, Properties.Settings.Default.XpsVw); Properties.Settings.Default.Save();
			base.OnClosed(e);
		}
	}
}
