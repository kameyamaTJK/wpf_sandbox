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

        public string BBB
        {
            get { return (string)GetValue(BBBProperty); }
            set { SetValue(BBBProperty, value); }
        }
        public static readonly DependencyProperty BBBProperty =
            DependencyProperty.Register("BBB", typeof(string), typeof(SampleData), new PropertyMetadata(string.Empty));
    }

    public class ContentPresenterCellFactory : ICellControlFactory
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
                new SampleData() { AAA = "SampleData 1", BBB = "SAMPLE-DATA A" },
                new SampleData() { AAA = "SampleData 2", BBB = "SAMPLE-DATA B" },
                new SampleData() { AAA = "SampleData 3", BBB = "SAMPLE-DATA C" },
                new SampleData() { AAA = "SampleData 4", BBB = "SAMPLE-DATA D" },
                new SampleData() { AAA = "SampleData 5", BBB = "SAMPLE-DATA E" },
            };
        }
    }
}
