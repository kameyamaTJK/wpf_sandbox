using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfSandbox
{
    public class CellInfo
    {
        public int Column { get; set; } = 0;
        public int ColumnSpan { get; set; } = 1;
        public Binding Bind { get; set; }
    }

    public class TextCellFactory : FreeCellGrid.ICellControlFactory
    {
        public Style Style { get; set; }

        public FrameworkElement CreateCellControl(object data)
        {
            var cellControl = new TextBlock() { Style = Style, DataContext = data };
            cellControl.SetBinding(TextBlock.TextProperty, new Binding("AAA"));
            return cellControl;
        }
    }

    public class FreeCellGrid : Grid
    {
        public interface ICellControlFactory
        {
            FrameworkElement CreateCellControl(object data);
        }

        public ICellControlFactory CellControlFactory { get; set; } = new TextCellFactory();

        public System.Collections.IEnumerable ItemsSource
        {
            get { return (System.Collections.IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(System.Collections.IEnumerable), typeof(FreeCellGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(onItemsSourceChanged)));

        private static void onItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = d as FreeCellGrid;
            if (owner != null)
            {
                owner.Reflesh();
            }
        }

        public List<CellInfo> CellInfos
        {
            get { return (List<CellInfo>)GetValue(CellInfosProperty); }
            set { SetValue(CellInfosProperty, value); }
        }
        public static readonly DependencyProperty CellInfosProperty =
            DependencyProperty.Register("CellInfos", typeof(List<CellInfo>), typeof(FreeCellGrid), new PropertyMetadata(new List<CellInfo>()));

        public override void EndInit()
        {
            base.EndInit();

            Reflesh();
        }

        public void Reflesh()
        {
            if (ItemsSource == null || CellInfos == null)
            {
                return;
            }

            RefleshCells(ItemsSource, CellInfos);
        }

        private void RefleshCells(System.Collections.IEnumerable items, IEnumerable<CellInfo> cells)
        {
            RowDefinitions.Clear();

            int row = 0;
            foreach (var item in items)
            {
                RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Auto)});

                foreach (var cell in cells)
                {
                    var cellControl = CellControlFactory.CreateCellControl(item);
                    if (cellControl == null)
                    {
                        Debug.Assert(true, "CellControlFactory.CreateCellControl() is failed.");
                        return;
                    }

                    SetRow(cellControl, row);
                    SetRowSpan(cellControl, 1);
                    SetColumn(cellControl, cell.Column);
                    SetColumnSpan(cellControl, cell.ColumnSpan);

                    Children.Add(cellControl);
                }
                row++;
            }
        }
    }
}
