using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;
using _41_размер.Classes;
using _41_размер.Entities;
using _41_размер.UserControls;

namespace _41_размер.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProductsWindow.xaml.
    /// </summary>
    public partial class ProductsWindow : Window
    {
        /// <summary>
        /// Список с товарами, которые соответствуют текущему фильтру.
        /// </summary>
        public List<Product> SelectedProducts { get; private set; } = TradeEntities.Instance.Products.ToList();

        #region Область: Функции инициализации.

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public ProductsWindow()
        {
            InitializeComponent();

            InitializeVisibility();
            InitializeListeners();
            UpdatePages();
        }

        /// <summary>
        /// Проверяет текущего пользователя на уровень доступа и скрывает определенные элементы.
        /// <br/>
        /// По умолчанию все элементы открыты — это сделано для удобства проектирования системы в конструкторе.
        /// В ходе работы эти элементы скрываются.
        /// </summary>
        private void InitializeVisibility()
        {
            Int32 role = SessionData.CurrentUser.UserRole;

            CheckRoleAndHideElements(role);
            CurrentOrder_Button.Visibility = SessionData.CurrentOrder.OrderProducts.Count > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// Проверяет уровень роли и скрывает определенные элементы.
        /// </summary>
        /// <param name="role">Текущий уровень пользователя.</param>
        private void CheckRoleAndHideElements(Int32 role)
        {
            if (role != 3)
            {
                CreateNewProduct_Button.Visibility = Visibility.Hidden;

                if (role != 2)
                {
                    AllOrders_Button.Visibility = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Инициализирует обработчики событий.
        /// </summary>
        private void InitializeListeners()
        {
            SearchBox.TextChanged += SelectionParametersChanged;

            SelectSort.SelectedIndex = 0;
            SelectSort.SelectionChanged += SelectionParametersChanged;

            SelectFilter.SelectedIndex = 0;
            SelectFilter.SelectionChanged += SelectionParametersChanged;

            SelectPage.SelectionChanged += SelectPage_SelectionChanged;
        }
        #endregion

        #region Область: Функции обновления выборки.

        /// <summary>
        /// Событие смены какого-либо параметра выборки данных.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void SelectionParametersChanged(Object sender, dynamic e)
        {
            SelectedProducts = TradeEntities.Instance.Products.ToList();

            // Поиск.
            if (SearchBox.Text is var search && !String.IsNullOrEmpty(search))
            {
                SelectedProducts = SelectedProducts.Where(p =>
                                   p.ProductName.Contains(search) ||
                                   (p.ProductDescription != null && p.ProductDescription.Contains(search))).ToList();
            }

            // Сортировка.
            if (SelectSort.SelectedIndex != 0)
            {
                SelectedProducts = SelectSort.SelectedIndex == 2 ?
                                   SelectedProducts.OrderBy(x => x.FinalCost).ToList() :
                                   SelectedProducts.OrderByDescending(p => p.FinalCost).ToList();
            }

            // Фильтрация.
            if (SelectFilter.SelectedIndex != 0)
            {
                switch (SelectFilter.SelectedIndex)
                {
                    case 1:
                        SelectedProducts = SelectedProducts.Where(p =>
                                           p.ProductDiscountAmount > 0 &&
                                           p.ProductDiscountAmount < 10).ToList();

                        break;

                    case 2:
                        SelectedProducts = SelectedProducts.Where(p =>
                                           p.ProductDiscountAmount > 9.99 &&
                                           p.ProductDiscountAmount < 15).ToList();

                        break;

                    default:
                        SelectedProducts = SelectedProducts.Where(p =>
                                           p.ProductDiscountAmount > 15).ToList();

                        break;
                }
            }

            UpdatePages();

            // Уведомление о пустом результате поиска.
            if (SelectedProducts.Count == 0)
            {
                MessageBox.Show("По вашему запросу ничего не найдено.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Обновляет список страниц и сбрасывает выбранную страницу на 1.
        /// </summary>
        private void UpdatePages()
        {
            Int32 pages = SelectedProducts.Count / 20;

            if (SelectedProducts.Count % 20 > 0 || SelectedProducts.Count == 0)
                pages++;

            List<Int32> pagesList = Enumerable.Range(1, pages).ToList();
            SelectPage.ItemsSource = pagesList;

            if (SelectPage.SelectedIndex != 0)
                SelectPage.SelectedIndex = 0;

            else
                SelectPage_SelectionChanged(default, default);
        }
        #endregion

        #region Область: Функции навигации.

        /// <summary>
        /// Событие перемещения пользователя на предыдущую страницу.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void GoToPreviousPage_Click(Object sender, RoutedEventArgs e)
        {
            Int32 pageIndex = SelectPage.SelectedIndex;

            if (pageIndex > 0)
                SelectPage.SelectedIndex--;
        }

        /// <summary>
        /// Событие выбора страницы для перехода.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void SelectPage_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            Int32 currentPageIndex = SelectPage.SelectedIndex;
            List<Product> currentPageProducts = SelectedProducts.Skip(20 * currentPageIndex).Take(20).ToList();

            UpdateProductsListView(currentPageProducts);
            UpdateCurrentDisplayedProductsTitle(currentPageIndex, currentPageProducts.Count);
        }

        /// <summary>
        /// Событие перехода на следующую страницу.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void GoToNextPage_Click(Object sender, RoutedEventArgs e)
        {
            Int32 pageIndex = SelectPage.SelectedIndex;

            if (pageIndex < SelectPage.Items.Count - 1)
                SelectPage.SelectedIndex++;
        }
        #endregion

        #region Область: Функции обновления данных о товарах.

        /// <summary>
        /// Обновляет список с товарами.
        /// </summary>
        /// <param name="productsToDisplay">Товары, которые нужно вставить в окно.</param>
        private void UpdateProductsListView(List<Product> productsToDisplay)
        {
            Double width = GetOptimalWidthForListItems();
            ProductsList.Items.Clear();

            foreach (Product product in productsToDisplay)
            {
                ProductListItem item = new ProductListItem(product)
                {
                    Width = width
                };
                item.AddToCart.Click += CartChanged;
                item.RemoveFromCart.Click += CartChanged;

                ProductsList.Items.Add(item);
            }
        }

        /// <summary>
        /// Обновляет заголовок с информацией о текущих выведенных товарах.
        /// </summary>
        /// <param name="currentPageIndex">Индекс текущей страницы.</param>
        /// <param name="productsQuantityOnCurrentPage">Количество товаров на текущей странице.</param>
        private void UpdateCurrentDisplayedProductsTitle(Int32 currentPageIndex, Int32 productsQuantityOnCurrentPage)
        {
            CurrentProducts.Text = productsQuantityOnCurrentPage > 0 ?
                                   $"{currentPageIndex * 20 + 1} — {currentPageIndex * 20 + productsQuantityOnCurrentPage} / {SelectedProducts.Count}" :
                                   "N/A";
        }
        #endregion

        #region Область: Функции работы с товарами.

        /// <summary>
        /// Событие изменения корзины.
        /// <br/>
        /// К обычному обработчику события для нажатия на пункт контекстного меню добавляется ещё один обработчик.
        /// Это позволяет динамически обновлять состояние кнопки отображения текущего заказа при изменении товаров.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void CartChanged(Object sender = null, EventArgs e = null)
        {
            if (SessionData.CurrentOrder.OrderProducts.Count > 0)
            {
                CurrentOrder_Button.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Событие двойного нажатия на элемент списка с товарами.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ProductsList_MouseDoubleClick(Object sender, MouseButtonEventArgs e)
        {
            if (CheckUserRoleToGainAccess(3) && ProductsList.SelectedItem != null)
            {
                Product selected = (ProductsList.SelectedItem as ProductListItem)?.CurrentProduct;
                Product clone = selected.Clone() as Product;

                EditProductWindow newWindow = new EditProductWindow(clone);
                Boolean? result = newWindow.ShowDialog();

                if (result.HasValue && result.Value)
                {
                    if (!newWindow.DeleteThisProduct)
                    {
                        selected.Merge(clone);
                        TradeEntities.Instance.SaveChanges();

                        MessageBox.Show("Изменения сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        SelectPage_SelectionChanged(default, default);
                    }

                    else
                    {
                        TradeEntities.Instance.Products.Remove(selected);
                        TradeEntities.Instance.OrderProducts.RemoveRange(TradeEntities.Instance.OrderProducts.Where(p => p.Product.ProductArticleNumber == selected.ProductArticleNumber));
                        TradeEntities.Instance.Orders.RemoveRange(TradeEntities.Instance.Orders.Where(o => o.OrderProducts.Count < 1));

                        TradeEntities.Instance.SaveChanges();
                        MessageBox.Show("Товар удален.\n\nСвязанные заказы обновлены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                        SelectionParametersChanged(default, default);
                    }
                }
            }
        }
        #endregion

        #region Область: События кнопок нижней панели.

        /// <summary>
        /// Событие перехода к окну настройки текущего заказа.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void CurrentOrder_Button_Click(Object sender, RoutedEventArgs e)
        {
            OrderFormation window = new OrderFormation();
            window.ShowDialog();

            CartChanged();
        }

        /// <summary>
        /// Событие перехода к окну всех заказов.
        /// <br/>
        /// Доступно для менеджера и администратора.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void AllOrders_Button_Click(Object sender, RoutedEventArgs e)
        {
            if (CheckUserRoleToGainAccess(2, 3))
            {
                Hide();

                OrdersWindow newWindow = new OrdersWindow();
                Boolean? result = newWindow.ShowDialog();

                if (result.HasValue && result.Value)
                {
                    TradeEntities.Instance.SaveChanges();
                }

                CartChanged();
                Show();
            }
        }

        /// <summary>
        /// Событие создания нового товара.
        /// <br/>
        /// Доступно для администратора.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void CreateNewProduct_Button_Click(Object sender, RoutedEventArgs e)
        {
            if (CheckUserRoleToGainAccess(3))
            {
                EditProductWindow newWindow = new EditProductWindow(null);
                Boolean? result = newWindow.ShowDialog();

                if (result.HasValue && result.Value)
                {
                    TradeEntities.Instance.Products.Add(newWindow.EditingProduct);
                    TradeEntities.Instance.SaveChanges();

                    MessageBox.Show("Товар добавлен.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelectionParametersChanged(default, default);
                }
            }
        }
        #endregion

        #region Область: Прочие функции.

        /// <summary>
        /// Функция, выполняющая проверку текущей роли пользователя на доступность к указанному функционалу.
        /// <br/>
        /// Выводит окно с ошибкой, если роль недостаточно высокая.
        /// </summary>
        /// <param name="allowedRoles">Список "подходящих" ролей.</param>
        /// <returns>Соответствие текущей роли пользователя к указанным требованиям.</returns>
        private Boolean CheckUserRoleToGainAccess(params Int32[] allowedRoles)
        {
            if (allowedRoles.Contains(SessionData.CurrentUser.UserRole))
            {
                return true;
            }

            else
            {
                MessageBox.Show("I just don't get it;\nWhy do you want to break the program?",
                                "You should find another work", MessageBoxButton.OK, MessageBoxImage.Warning);

                return false;
            }
        }

        /// <summary>
        /// Событие смены размера окна.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_SizeChanged(Object sender, SizeChangedEventArgs e)
        {
            foreach (ProductListItem item in ProductsList.Items)
            {
                item.Width = GetOptimalWidthForListItems();
            }
        }

        /// <summary>
        /// Получает оптимальный размер для элемента списка товаров.
        /// </summary>
        /// <returns>Оптимальный размер.</returns>
        private Double GetOptimalWidthForListItems()
        {
            Double result = WindowState == WindowState.Maximized ? RenderSize.Width : Width;

            return result - 55;
        }

        /// <summary>
        /// Событие закрытия окна. Переводит пользователя обратно в окно выбора аккаунта.
        /// <br/>
        /// Раз программа предназначена для исполнения на терминале, то она не должна закрываться. 
        /// По крайней мере, обычными способами.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Closing(Object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
        }
        #endregion
    }
}
