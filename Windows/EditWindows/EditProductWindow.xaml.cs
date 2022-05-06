using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using _41_размер.Entities;
using _41_размер.Resources;

namespace _41_размер.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditProductWindow.xaml.
    /// </summary>
    public partial class EditProductWindow : Window
    {
        #region Область: Поля и свойства.

        /// <summary>
        /// Поле, отвечающее за то, изменялось ли изображение товара.
        /// </summary>
        private Boolean imageChanged = false;

        /// <summary>
        /// Свойство, отвечающее за то редактируется заказ или создается.
        /// </summary>
        public Boolean Creating { get; private set; }

        /// <summary>
        /// Свойство, отвечающее за то, будет ли удален товар при закрытии окна.
        /// </summary>
        public Boolean DeleteThisProduct { get; private set; }

        /// <summary>
        /// Свойство с изменяемым продуктом.
        /// </summary>
        public Product EditingProduct { get; set; }
        #endregion

        #region Область: Функции инициализации.

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public EditProductWindow(Product toEdit)
        {
            EditingProduct = toEdit ?? new Product();
            Creating = EditingProduct == null;

            InitializeComponent();
            DataContext = EditingProduct;
        }

        /// <summary>
        /// Событие загрузки страницы.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            EditManufacturer.ItemsSource = TradeEntities.Instance.Companies.ToList();
            EditDeliver.ItemsSource = TradeEntities.Instance.Companies.ToList();

            EditManufacturer.SelectedItem = EditingProduct.Company ?? TradeEntities.Instance.Companies.First();
            EditDeliver.SelectedItem = EditingProduct.Company1 ?? TradeEntities.Instance.Companies.First();
        }
        #endregion

        #region Область: События изменения товара.

        /// <summary>
        /// Событие смены выбранной компании-производителя товара.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void EditManufacturer_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            Company selected = EditManufacturer.SelectedItem as Company;

            EditingProduct.ProductManufacturerId = selected.CompanyId;
            EditingProduct.Company = selected;
        }

        /// <summary>
        /// События смены выбранной компании-поставщика товара.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void EditDeliver_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            Company selected = EditDeliver.SelectedItem as Company;

            EditingProduct.ProductDeliver = selected.CompanyId;
            EditingProduct.Company1 = selected;
        }

        /// <summary>
        /// Событие изменения изображения товара.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void EditProductPhoto_Click(Object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Title = "Выбор изображения",
                Filter = "Формат .jpg|*.jpg|Формат .png|*.png|Формат .jpeg|*.jpeg",
                Multiselect = false
            };
            Boolean? result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                imageChanged = true;

                EditingProduct.ProductPhoto = dialog.FileName;
                ShowProductPhoto.Source = new BitmapImage(new Uri(dialog.FileName));
            }
        }
        #endregion

        #region Область: События контрольных кнопок.

        /// <summary>
        /// Событие удаления товара.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void DeleteProduct_Click(Object sender, RoutedEventArgs e)
        {
            if (Creating)
            {
                MessageBox.Show("Нельзя удалить ещё не созданный товар.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                MessageBoxResult result = MessageBox.Show("Вы ТОЧНО уверены?\n\nЭто действие необратимо.", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    DialogResult = true;
                    DeleteThisProduct = true;

                    Close();
                }
            }
        }

        /// <summary>
        /// Событие сохранения товара.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void SaveProduct_Click(Object sender, RoutedEventArgs e)
        {
            if (CheckToErrors())
            {
                if (imageChanged)
                    EditingProduct.ProductPhoto = ResourceManager.SaveImageToResources(EditingProduct.ProductPhoto);

                DialogResult = true;
                Close();
            }
        }
        #endregion

        #region Область: Прочие функции.

        /// <summary>
        /// Функция, проверяющая введенные данные на корректность.
        /// <br/>
        /// Если данные некорректны, она выведет данные об ошибках.
        /// </summary>
        /// <returns>Корректность введенных данных.</returns>
        private Boolean CheckToErrors()
        {
            String error = String.Empty;

            if (!CheckToUniqueArticleNumber() || String.IsNullOrEmpty(EditArticleNumber.Text))
                error += "Некорректный номер артикула (он не может быть пустым или повторяться).\n";

            if (String.IsNullOrEmpty(EditProductName.Text))
                error += "Название товара не может быть пустым.\n";

            if (String.IsNullOrEmpty(EditProductUnit.Text))
                error += "Единица измерения товара не может быть пустой.\n";

            if (String.IsNullOrEmpty(EditMaxDiscount.Text) || !Int32.TryParse(EditMaxDiscount.Text, out Int32 tmp))
                error += "Значение максимальной скидки не может быть некорректным (для отсутствия скидки введите '0').\n";

            if (String.IsNullOrEmpty(EditProductDiscountAmount.Text) || !Int32.TryParse(EditProductDiscountAmount.Text, out Int32 tmp1))
                error += "Значение текущей скидки не может быть некорректным (для отсутствия скидки введите '0').\n";

            if (EditingProduct.ProductMaxDiscount < EditingProduct.ProductDiscountAmount)
                error += "Текущая скидка не может быть больше максимальной скидки.\n";

            if (String.IsNullOrEmpty(EditQuantityInStock.Text) || !Int32.TryParse(EditQuantityInStock.Text, out Int32 tmp2) || EditingProduct.ProductQuantityInStock < 0)
                error += "Некорректное количество остатка на складе.\n";

            if (String.IsNullOrEmpty(EditProductCost.Text) || !Decimal.TryParse(EditProductCost.Text.Replace('.', ','), out Decimal tmp3) || EditingProduct.FinalCost < 0M)
                error += "Некорректная стоимость товара.\n";

            if (String.IsNullOrEmpty(EditProductCategory.Text))
                error += "У товара должна быть указана категория.\n";

            if (error == String.Empty)
            {
                return true;
            }

            else
            {
                MessageBox.Show($"Обнаружены ошибки:\n{error}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
        }

        /// <summary>
        /// Проверяет уникальность введенного номера артикула.
        /// </summary>
        /// <returns>Уникальность артикула.</returns>
        private Boolean CheckToUniqueArticleNumber()
        {
            if (Creating)
            {
                return !TradeEntities.Instance.Products.Any(p => p.ProductArticleNumber == EditingProduct.ProductArticleNumber);
            }

            else
            {
                return TradeEntities.Instance.Products.Count(p => p.ProductArticleNumber == EditingProduct.ProductArticleNumber) < 2;
            }
        }
        #endregion
    }
}
