using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace lab9
{
    public partial class AddProductEdit : Window
    {
        private Product _currentProduct;
        private bool _isEditMode;
        
        public AddProductEdit()
        {
            InitializeComponent();
            _isEditMode = false;
            Title = "Добавление товара";
        }
        
        public AddProductEdit(Product product)
        {
            InitializeComponent();
            _currentProduct = product;
            _isEditMode = true;
            Title = "Редактирование товара";
            
            LoadProductData();
        }

        private void LoadProductData()
        {
            if (_currentProduct != null)
            {
                PArticle.Text = _currentProduct.ProductArticleNumber;
                PArticle.IsEnabled = false;
                
                PName.Text = _currentProduct.ProductName;
                PDesc.Text = _currentProduct.ProductDescription;
                PProd.Text = _currentProduct.ProductManufacturer;
                PCat.Text = _currentProduct.ProductCategory;
                PPrice.Text = _currentProduct.ProductCost.ToString();
                PMeasure.Text = _currentProduct.ProductQuantityInStock.ToString();
                PStatus.Text = _currentProduct.ProductStatus;
            }
        }

        private void AddButton(object sender, RoutedEventArgs e)
        {
            try
            { 
                var context = TradeEntities.GetContext();

                if (_isEditMode)
                {
                    _currentProduct.ProductName = PName.Text.Trim();
                    _currentProduct.ProductDescription = PDesc.Text.Trim();
                    _currentProduct.ProductManufacturer = PProd.Text.Trim();
                    _currentProduct.ProductCategory = PCat.Text.Trim();
                    _currentProduct.ProductCost = Convert.ToDecimal(PPrice.Text);
                    _currentProduct.ProductQuantityInStock = Convert.ToInt32(PMeasure.Text);
                    _currentProduct.ProductStatus = PStatus.Text.Trim();

                    MessageBox.Show("Товар успешно обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var existingProduct = context.Product
                        .FirstOrDefault(p => p.ProductArticleNumber == PArticle.Text.Trim());

                    if (existingProduct != null)
                    {
                        MessageBox.Show("Товар с таким артикулом уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var newProduct = new Product
                    {
                        ProductArticleNumber = PArticle.Text.Trim(),
                        ProductName = PName.Text.Trim(),
                        ProductDescription = PDesc.Text.Trim(),
                        ProductManufacturer = PProd.Text.Trim(),
                        ProductCategory = PCat.Text.Trim(),
                        ProductCost = Convert.ToDecimal(PPrice.Text),
                        ProductQuantityInStock = Convert.ToInt32(PMeasure.Text),
                        ProductStatus = PStatus.Text.Trim()
                    };

                    context.Product.Add(newProduct);
                    MessageBox.Show("Товар успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                context.SaveChanges();
                Close();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string errors = "Ошибки валидации:\n";
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errors += $"• {validationError.PropertyName}: {validationError.ErrorMessage}\n";
                    }
                }

                MessageBox.Show(errors, "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPhoto(object sender, RoutedEventArgs e)
        {
            try
            {
                var openDialog = new OpenFileDialog();
                openDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
                openDialog.ShowDialog();

                if (openDialog.CheckFileExists && openDialog.FileName.Length > 0)
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openDialog.FileName);
                    bitmap.DecodePixelWidth = 250;
                    bitmap.EndInit();
                    PImage.Source = bitmap;
                    _currentProduct.ProductPhoto = File.ReadAllBytes(openDialog.FileName);
                }
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки картинки", "Ошибка");
            }
        }
    }
}