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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfSandbox
{
    public class SampleData : DependencyObject
    {
        public string AAA
        {
            get { return (string)GetValue(AAAProperty); }
            set { SetValue(AAAProperty, value); }
        }
        public static readonly DependencyProperty AAAProperty =
            DependencyProperty.Register("AAA", typeof(string), typeof(SampleData), new PropertyMetadata(string.Empty));
    }

    public class ContentPresenterCellFactory : FreeCellGrid.ICellControlFactory
    {
        public DataTemplate Template { get; set; }

        public FrameworkElement CreateCellControl(object data)
        {
            return new ContentPresenter() { Content = data, ContentTemplate = Template };
        }
    }

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            freeCellGrid.ItemsSource = new List<SampleData>()
            {
                new SampleData() { AAA = "SampleData 1" },
                new SampleData() { AAA = "SampleData 2" },
                new SampleData() { AAA = "SampleData 3" },
                new SampleData() { AAA = "SampleData 4" },
                new SampleData() { AAA = "SampleData 5" },
            };
        }
    }
}
