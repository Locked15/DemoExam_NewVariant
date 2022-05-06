using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using _41_размер.Classes;
using _41_размер.Entities;
using _41_размер.Resources;

namespace _41_размер.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ProductListItem.xaml.
    /// </summary>
    public partial class ProductListItem : UserControl
    {
        /// <summary>
        /// Текущий продукт.
        /// </summary>
        public Product CurrentProduct { get; set; }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="product">Продукт, который нужно вывести.</param>
        public ProductListItem(Product product)
        {
            CurrentProduct = product;

            InitializeComponent();
            DataContext = CurrentProduct;
        }

        /// <summary>
        /// Событие завершения загрузки окна.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserControl_Loaded(Object sender, RoutedEventArgs e)
        {
            ProductImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(ResourceManager.GetSafeImagePath(CurrentProduct.ProductPhoto)));
            ProductManufacturerInfo.Text = $"Производитель: {CurrentProduct.Company.CompanyName}.";
            DiscountSize.Text = $"{CurrentProduct.ProductDiscountAmount}%";

            CheckDiscountAndUpdateWindow();
        }

        /// <summary>
        /// Проверяет скидку товара и обновляет окно и его компоненты.
        /// </summary>
        private void CheckDiscountAndUpdateWindow()
        {
            if (CurrentProduct.ProductDiscountAmount > 0)
            {
                ProductDiscountPrice.Visibility = Visibility.Visible;
                ProductOriginPrice.TextDecorations = TextDecorations.Strikethrough;

                if (CurrentProduct.ProductDiscountAmount > 15)
                {
                    Background = new BrushConverter().ConvertFromString("#7fff00") as Brush;
                }
            }
        }

        /// <summary>
        /// Событие добавления товара в корзину заказа.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void AddToCart_Click(Object sender, RoutedEventArgs e)
        {
            OrderProduct element = SessionData.CurrentOrder.OrderProducts.FirstOrDefault(op => 
                                   op.ProductArticleNumber == CurrentProduct.ProductArticleNumber);

            if (element == null)
            {
                SessionData.CurrentOrder.OrderProducts.Add(new OrderProduct()
                {
                    ProductArticleNumber = CurrentProduct.ProductArticleNumber,
                    Product = CurrentProduct,
                    CountInOrder = 1,
                    Order = SessionData.CurrentOrder
                });
            }

            else
            {
                element.CountInOrder++;
            }
        }

        /// <summary>
        /// Событие удаления товара из корзины.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void RemoveFromCart_Click(Object sender, RoutedEventArgs e)
        {
            ICollection<OrderProduct> tethers = SessionData.CurrentOrder.OrderProducts;

            tethers.Remove(tethers.FirstOrDefault(p => p.ProductArticleNumber == CurrentProduct.ProductArticleNumber));
        }
    }
}
