using System;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Xps.Packaging;

namespace TimeTracker.Common
{
  public static class XamlToXps
  {
    public static void Export(Uri uri, FrameworkElement surface)
    {
      try
      {
        if (uri == null) return;
        var transform = surface.LayoutTransform;       // Save current canvas transorm
        surface.LayoutTransform = null;                      // Temporarily reset the layout transform before saving
        var size = new Size(surface.Width, surface.Height); // Get the size of the canvas
        surface.Measure(size);
        surface.Arrange(new Rect(size));

        if (File.Exists(uri.LocalPath)) File.Delete(uri.LocalPath);

        var package = Package.Open(uri.LocalPath, FileMode.Create);
        var doc = new XpsDocument(package);
        var writer = XpsDocument.CreateXpsDocumentWriter(doc);
        writer.Write(surface);
        doc.Close();
        package.Close();
        surface.LayoutTransform = transform;
      }
      catch (Exception ex) { MessageBox.Show(ex.ToString(), "Exception has been thrown", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
    //public static FlowDocument CreateFlowDocumentWithImage(string imageurl)
    //{
    //	Image LogoImage = CreateImage(imageurl);
    //	BlockUIContainer container = new BlockUIContainer();
    //	container.Child = LogoImage;
    //	Floater floater = new Floater(container);
    //	Paragraph para = new Paragraph();
    //	para.Inlines.Add(floater);
    //	FlowDocument doc = new FlowDocument();
    //	doc.Blocks.Add(para);
    //	return doc;
    //}
    //static Image CreateImage(string imageurl)
    //{
    //	string logourl = imageurl;
    //	BitmapImage bmi = new BitmapImage(new Uri(logourl, UriKind.Relative));
    //	Image LogoImage = new Image();
    //	FileStream stream = File.Open(logourl, FileMode.Open);
    //	LogoImage.Source = GetImage(stream);
    //	LogoImage.Margin = new Thickness(230, 30, 10, 10);
    //	LogoImage.Stretch = Stretch.UniformToFill;
    //	LogoImage.Width = 60;
    //	LogoImage.Height = 60;
    //	LogoImage.HorizontalAlignment = HorizontalAlignment.Center;
    //	return LogoImage;
    //}
    //static BitmapImage GetImage(Stream iconStream)
    //{
    //	System.Drawing.Image img = System.Drawing.Image.FromStream(iconStream);
    //	var imgbrush = new BitmapImage();
    //	imgbrush.BeginInit();
    //	imgbrush.StreamSource = ConvertImageToMemoryStream(img);
    //	imgbrush.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
    //	imgbrush.EndInit();
    //	var ib = new ImageBrush(imgbrush);
    //	return imgbrush;
    //}
    //static MemoryStream ConvertImageToMemoryStream(System.Drawing.Image img)
    //{
    //	var ms = new MemoryStream();
    //	img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
    //	return ms;
    //}
  }
}
