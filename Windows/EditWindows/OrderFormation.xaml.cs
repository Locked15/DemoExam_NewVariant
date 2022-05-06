using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using _41_размер.Classes;
using _41_размер.Entities;
using _41_размер.UserControls;

namespace _41_размер.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrderFormation.xaml.
    /// </summary>
    public partial class OrderFormation : Window
    {
        #region Область: Поля.

        /// <summary>
        /// Поле, отвечающее за то, будут ли сохранены изменения в БД или только в локальном списке, при нажатии на кнопку.
        /// </summary>
        private Boolean saveChangesToDb;
        #endregion

        #region Область: Свойства.

        /// <summary>
        /// Свойство, содержащее ссылку на данные текущего пользователя.
        /// <br/>
        /// Нужно для привязки данных.
        /// </summary>
        public User CurrentUser { get; private set; }

        /// <summary>
        /// Свойство, содержащее ссылку на данные текущего заказа.
        /// <br/>
        /// Нужно для привязки данных.
        /// </summary>
        public Order CurrentOrder { get; private set; }
        #endregion

        #region Область: Функции инициализации.

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public OrderFormation(Boolean saveChanges = true)
        {
            saveChangesToDb = saveChanges;
            CurrentUser = SessionData.CurrentUser;
            CurrentOrder = SessionData.CurrentOrder;

            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Событие завершения загрузки окна.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            PickerPoint.ItemsSource = TradeEntities.Instance.OrderPickupPoints.ToList();
            PickerPoint.SelectedIndex = TradeEntities.Instance.OrderPickupPoints.ToList().FindIndex(p => p.PointId == CurrentOrder.OrderPickupPointId);

            SelectDeliveryDate.DisplayDateStart = DateTime.Now;
            SelectDeliveryDate.SelectedDate = CurrentOrder.OrderDeliveryDate;

            CheckUserRole();
            UpdateProductsListView();

            MessageBox.Show("Для удаления товара используйте контекстное меню.\nТакже вы можете использовать его для увеличения кол-ва товара в заказе.",
                            "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Выполняет проверку уровня доступа пользователя.
        /// <br/>
        /// Если пользователь — гость, то скрывает данные об аккаунте.
        /// </summary>
        private void CheckUserRole()
        {
            if (CurrentOrder.User == null || CurrentOrder.User.UserRole == 4)
            {
                UserInfoDesc.Visibility = Visibility.Hidden;
                UserInfo.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        #region Область: События обновления отображаемых данных.

        /// <summary>
        /// Обновляет список товаров.
        /// <br/>
        /// Параметры добавлены, чтобы данную функцию можно было использовать как обработчик события.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UpdateProductsListView(Object sender = null, EventArgs e = null)
        {
            UpdateOrderInfoElements();

            if (CurrentOrder.OrderProducts.Any())
            {
                ProductsInOrder_ListView.Items.Clear();

                foreach (Product product in CurrentOrder.OrderProducts.Select(p => p.Product).ToList())
                {
                    ProductListItem item = new ProductListItem(product)
                    {
                        Width = GetOptimalItemWidth()
                    };
                    item.RemoveFromCart.Click += UpdateProductsListView;
                    item.AddToCart.Click += (Object sDef, RoutedEventArgs def) =>
                    {
                        ProductsInOrder_ListView.SelectedIndex = -1;

                        UpdateProductsListView();
                    };

                    ProductsInOrder_ListView.Items.Add(item);
                }
            }

            else
            {
                // Удаляем заказ из БД, если в нем не осталось товаров.
                try
                {
                    TradeEntities.Instance.Orders.Remove(CurrentOrder);
                    TradeEntities.Instance.SaveChanges();
                }

                // Но если заказа в БД ещё нет, то это вызовет ошибку и её нужно отловить.
                catch
                {

                }

                Close();
            }
        }

        /// <summary>
        /// Обновляет элементы с данными о заказе.
        /// </summary>
        private void UpdateOrderInfoElements()
        {
            if (CurrentOrder.OrderProducts.Any())
            {
                OrderFinalCost.Text = Math.Round(CurrentOrder.FinalCost, 2).ToString();
                OrderFinalDiscount.Text = CurrentOrder.FinalDiscount;
            }
        }
        #endregion

        #region Область: События изменения товаров.

        /// <summary>
        /// Событие изменения выбранного элемента в списке товаров.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ProductsInOrder_ListView_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (ProductsInOrder_ListView.SelectedItem != null)
            {
                try
                {
                    OrderProduct temp = CurrentOrder.OrderProducts.First(p =>
                                        p.ProductArticleNumber.Equals((ProductsInOrder_ListView.SelectedItem as ProductListItem)?.CurrentProduct.ProductArticleNumber));

                    CurrentProductCount.Text = temp.CountInOrder.ToString();
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка...\n\n{ex.Message}.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            else
            {
                CurrentProductCount.Text = String.Empty;
            }
        }

        /// <summary>
        /// Событие изменения текста в поле ввода количества товара.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void CurrentProductCount_TextChanged(Object sender, TextChangedEventArgs e)
        {
            if (ProductsInOrder_ListView.SelectedItem != null)
            {
                if (String.IsNullOrEmpty(CurrentProductCount.Text))
                {
                    MessageBox.Show("Пустое значение не допускается.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);

                    CurrentProductCount.Text = "1";
                }

                else if (Int32.TryParse(CurrentProductCount.Text, out Int32 number))
                {
                    if (number < 1)
                    {
                        MessageBoxResult result = MessageBox.Show($"Подобное значение ({number}) приведет к удалению товара из заказа.\n\nВы уверены?",
                                                                   "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            CurrentOrder.OrderProducts.Remove(CurrentOrder.OrderProducts.First(p =>
                                         p.ProductArticleNumber == (ProductsInOrder_ListView.SelectedItem as ProductListItem)?.CurrentProduct.ProductArticleNumber));

                            UpdateProductsListView();
                        }

                        else
                        {
                            CurrentProductCount.Text = CurrentProductCount.Text.Replace("-", String.Empty);
                        }
                    }

                    else
                    {
                        OrderProduct op = CurrentOrder.OrderProducts.First(p =>
                                     p.ProductArticleNumber == (ProductsInOrder_ListView.SelectedItem as ProductListItem)?.CurrentProduct.ProductArticleNumber);

                        op.CountInOrder = number;
                    }
                }

                else
                {
                    MessageBox.Show("Некорректное значение.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);

                    CurrentProductCount.Text = "1";
                }

                UpdateOrderInfoElements();
            }
        }

        /// <summary>
        /// Событие изменения точки выдачи товара.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void PickerPoint_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (PickerPoint.SelectedItem is OrderPickupPoint selected && selected != null)
            {
                CurrentOrder.OrderPickupPointId = selected.PointId;
                CurrentOrder.OrderPickupPoint = selected;
            }
        }
        #endregion

        #region Область: События нижней панели.

        /// <summary>
        /// Событие генерации накладной в формате "PDF".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void GeneratePdfDocumentation_Click(Object sender, RoutedEventArgs e) =>
                     MessageBox.Show("Данный функционал ещё не реализован.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);

        /// <summary>
        /// Событие сохранения заказа в БД.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void SaveOrderInDb_Click(Object sender, RoutedEventArgs e)
        {
            if (CheckDataToCorrect())
            {
                String message = saveChangesToDb ? "Данные сохранены в Базе Данных." : "Данные сохранены в промежуточных изменениях.";

                if (CurrentOrder.OrderID == 0)
                {
                    try
                    {
                        TradeEntities.Instance.Orders.Add(CurrentOrder);
                        TradeEntities.Instance.OrderProducts.AddRange(CurrentOrder.OrderProducts);

                        if (saveChangesToDb)
                            TradeEntities.Instance.SaveChanges();

                        MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show($"При сохранении данных в БД произошла ошибка.\n\n{ex.Message}.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                else
                {
                    if (saveChangesToDb)
                        TradeEntities.Instance.SaveChanges();

                    MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Проверяет корректность введенных данных.
        /// </summary>
        /// <returns>Логическое значение корректности.</returns>
        private Boolean CheckDataToCorrect()
        {
            String error = String.Empty;

            if (PickerPoint.SelectedIndex == -1)
            {
                error += "Место получения заказа не выбрано.\n";
            }

            if (String.IsNullOrEmpty(error))
            {
                return true;
            }

            else
            {
                MessageBox.Show($"Обнаружены ошибки:\n{error}", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);

                return false;
            }
        }
        #endregion

        #region Область: Прочие функции.

        /// <summary>
        /// Событие изменения размеров окна.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_SizeChanged(Object sender, SizeChangedEventArgs e)
        {
            Double size = GetOptimalItemWidth();

            foreach (ProductListItem item in ProductsInOrder_ListView.Items)
            {
                item.Width = size;
            }
        }

        /// <summary>
        /// Получает оптимальную ширину для элементов списка товаров.
        /// </summary>
        /// <returns></returns>
        private Double GetOptimalItemWidth()
        {
            Double result = WindowState == WindowState.Maximized ? RenderSize.Width - 55 : Width - 55;

            return result;
        }
        #endregion
    }
}
