using System;
using System.Linq;
using System.Windows;

namespace lab9
{
    public partial class AddProductEdit : Window
    {
        public AddProductEdit()
        {
            InitializeComponent();
        }

        private void AddButton(object sender, RoutedEventArgs e)
        {
            try
            {
                var newProduct = new Product
                {
                    ProductArticleNumber = "DA223D",
                    ProductName = "qweqweqwe",
                    ProductDescription = "qweqweqwe",
                    ProductManufacturer = "qweqweqwe",
                    ProductCategory = "qweqweqwe",
                    ProductCost = Convert.ToDecimal(1112),
                    ProductMeasure = Convert.ToInt32(2),
                    ProductStatus = "qweqweqwe"
                };
            
                TradeEntities.GetContext().Product.Add(newProduct);
                TradeEntities.GetContext().SaveChanges();
            
                MessageBox.Show("Товар успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            } catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string errors = "";
                foreach (var validationErrors in ex.EntityValidationErrors)
                foreach (var validationError in validationErrors.ValidationErrors)
                    errors += $"{validationError.PropertyName}: {validationError.ErrorMessage}\n";

                MessageBox.Show(errors);
            }
        }
    }
}