using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TimeTracker.View
{
  public partial class FallbackEditor : Window
  {
    readonly string _text;

    public FallbackEditor(string text)
    {
      InitializeComponent();

      tbx.Text = _text = text;
    }
  }
}
