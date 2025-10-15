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
                    ProductArticleNumber = PArticle.Text,
                    ProductName = PName.Text,
                    ProductDescription = PDesc.Text,
                    ProductManufacturer = PProd.Text,
                    ProductCategory = PCat.Text,
                    ProductCost = Convert.ToDecimal(PPrice.Text),
                    ProductMeasure = Convert.ToInt32(PMeasure.Text),
                    ProductStatus = PStatus.Text
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