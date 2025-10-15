using System.Linq;
using System.Windows;

namespace lab9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            ProductsList.ItemsSource = TradeEntities.GetContext().Product.ToList();
        }

        private void AddProductButton(object sender, RoutedEventArgs e)
        {
            AddProductEdit addProduct = new AddProductEdit();
            addProduct.ShowDialog();
            ReloadProducts(null, null);
        }

        private void ReloadProducts(object sender, RoutedEventArgs e)
        {
            TradeEntities.ResetContext();
            LoadProducts();
            MessageBox.Show("Reloaded");
        }
    }
}