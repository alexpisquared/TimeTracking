using System.Windows;
using System.Windows.Media.Imaging;

namespace TimeTracker.View
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 // : AAV.WPF.Base.WindowBase
    {
        public Window2()
        {
            InitializeComponent();
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            Rect rect = new Rect((int)canvas.Margin.Left, (int)canvas.Margin.Top, (int)(canvas.ActualWidth + 2 * canvas.Margin.Left), (int)(canvas.ActualHeight + 2 * canvas.Margin.Top)); //new Rect(canvas.RenderSize);// Something that I’ve been struggling with in dealing with XAML - to - Image conversion code is positioning.If your canvas is positioned inside of a parent container in any way, you have to take that into account or else your canvas will be cut - off in the resulting image. For example, if your canvas is inside of a grid(as mine was), the first line needs to look more like:
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right, (int)rect.Bottom, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvas);

            var pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            pngEncoder.Save(ms);
            ms.Close();

            System.IO.File.WriteAllBytes("logo.png", ms.ToArray());
        }
    }
}
