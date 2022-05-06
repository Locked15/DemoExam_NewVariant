using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using _41_размер.Classes;
using _41_размер.Entities;
using _41_размер.UserControls;

namespace _41_размер.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml.
    /// </summary>
    public partial class OrdersWindow : Window
    {
        #region Область: Свойства.

        /// <summary>
        /// Свойство, нужное для привязки данных.
        /// </summary>
        public User CurrentUser { get; set; } = SessionData.CurrentUser;

        /// <summary>
        /// Свойство, содержащее исключаемые из выборки заказы.
        /// <br/>
        /// Это нужно для отложенного сохранения удаления заказов из БД.
        /// </summary>
        private List<Order> ExceptedOrders { get; set; } = new List<Order>(1);

        /// <summary>
        /// Свойство со свойствами, соответствующими текущим параметрам выборки.
        /// </summary>
        private List<Order> SelectedOrders { get; set; } = TradeEntities.Instance.Orders.ToList();
        #endregion

        #region Область: Функции инициализации.

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public OrdersWindow()
        {
            InitializeComponent();
            DataContext = this;

            InitializeListeners();
        }

        /// <summary>
        /// Функция инициализации обработчиков событий.
        /// <br/>
        /// Кроме того, вызывает событие изменения выбранного параметра селектора, что приводит к первичному заполнению списка с заказами.
        /// </summary>
        private void InitializeListeners()
        {
            SelectOrdersSort.SelectedIndex = 0;
            SelectOrdersSort.SelectionChanged += SelectionParametersChanged;

            SelectOrdersFilter.SelectionChanged += SelectionParametersChanged;
            SelectOrdersFilter.SelectedIndex = 0;
        }

        /// <summary>
        /// Событие загрузки страницы.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Loaded(Object sender, RoutedEventArgs e) =>
                MessageBox.Show("Чтобы редактировать заказ, нажмите на нем дважды.\nЧтобы удалить заказ или изменить его состояние, используйте контекстное меню." +
                                "\n\nПосле нажатия кнопки \"Сохранить изменения\" данное окно будет закрыто и указанные изменения будут сохранены в БД.",
                                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        #endregion

        #region Область: События обновления списка заказов.

        /// <summary>
        /// Событие изменения какого-либо параметра выборки заказов.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void SelectionParametersChanged(Object sender, dynamic e)
        {
            SelectedOrders = TradeEntities.Instance.Orders.ToList();

            // Сортировка.
            if (SelectOrdersSort.SelectedIndex > 0)
            {
                if (SelectOrdersSort.SelectedIndex == 2)
                    SelectedOrders = SelectedOrders.OrderBy(o => o.FinalCost).ToList();

                else
                    SelectedOrders = SelectedOrders.OrderByDescending(o => o.FinalCost).ToList();
            }

            // Фильтрация.
            if (SelectOrdersFilter.SelectedIndex > 0)
            {
                switch (SelectOrdersFilter.SelectedIndex)
                {
                    case 1:
                        SelectedOrders = SelectedOrders.Where(o =>
                                         Decimal.Parse(o.FinalDiscount.TrimEnd('%')) <= 10).ToList();

                        break;

                    case 2:
                        SelectedOrders = SelectedOrders.Where(o =>
                                         Decimal.Parse(o.FinalDiscount.TrimEnd('%')) > 10 &&
                                         Decimal.Parse(o.FinalDiscount.TrimEnd('%')) < 15).ToList();

                        break;

                    case 3:
                        SelectedOrders = SelectedOrders.Where(o =>
                                         Decimal.Parse(o.FinalDiscount.TrimEnd('%')) >= 15).ToList();

                        break;
                }
            }

            UpdateOrdersList();
        }

        /// <summary>
        /// Производит обновление списка заказов.
        /// </summary>
        private void UpdateOrdersList()
        {
            AllOrdersList.Items.Clear();

            foreach (Order order in SelectedOrders.Except(ExceptedOrders))
            {
                OrderListItem item = new OrderListItem(order)
                {
                    Width = GetOptimalItemWidth()
                };
                item.MainContextAction.Click += OrderListItemMainContextAction;
                item.SecondContextAction.Click += OrderListItemSecondContextAction;

                AllOrdersList.Items.Add(item);
            }
        }
        #endregion

        #region Область: События работы с заказами.

        /// <summary>
        /// Событие нажатия на основное контекстное действие элемента списка заказов.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void OrderListItemMainContextAction(Object sender, RoutedEventArgs e)
        {
            Order selected = (AllOrdersList.SelectedItem as OrderListItem)?.CurrentOrder;
            Order pointerToDbValue = TradeEntities.Instance.Orders.FirstOrDefault(o => o.OrderID == selected.OrderID);

            if (pointerToDbValue != null)
            {
                pointerToDbValue.OrderStatus = pointerToDbValue.OrderStatus == 1 ? 2 : 1;
                pointerToDbValue.Status = TradeEntities.Instance.Status.Where(s => s.StatusId == pointerToDbValue.OrderStatus).FirstOrDefault();

                UpdateOrdersList();
            }

            else
            {
                MessageBox.Show("В процессе обновления значения произошла ошибка.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Событие нажатия на вторичное контекстное действие элемента списка заказов.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void OrderListItemSecondContextAction(Object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы ТОЧНО уверены?\n\n(В случае ошибки это действие можно предотвратить, не сохраняя изменения).",
                                                      "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Order selected = (AllOrdersList.SelectedItem as OrderListItem)?.CurrentOrder;
                
                ExceptedOrders.Add(TradeEntities.Instance.Orders.Remove(TradeEntities.Instance.Orders.FirstOrDefault(o => o.OrderID == selected.OrderID)));
                TradeEntities.Instance.OrderProducts.RemoveRange(TradeEntities.Instance.OrderProducts.Where(op => op.OrderID == selected.OrderID));

                UpdateOrdersList();
            }
        }

        /// <summary>
        /// Событие двойного нажатия на элемент списка с заказами.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void AllOrdersList_MouseDoubleClick(Object sender, MouseButtonEventArgs e)
        {
            if (AllOrdersList.SelectedItem != null)
            {
                Order selected = (AllOrdersList.SelectedItem as OrderListItem)?.CurrentOrder;
                SessionData.CurrentOrder = TradeEntities.Instance.Orders.FirstOrDefault(o => o.OrderID == selected.OrderID);

                OrderFormation newWindow = new OrderFormation(false);
                newWindow.ShowDialog();

                UpdateOrdersList();
            }
        }
        #endregion

        #region Область: Функции изменения размера окна.

        /// <summary>
        /// Событие изменения размера окна.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_SizeChanged(Object sender, SizeChangedEventArgs e)
        {
            foreach (OrderListItem item in AllOrdersList.Items)
            {
                item.Width = GetOptimalItemWidth();
            }
        }

        /// <summary>
        /// Вычисляет оптимальный размер для элементов списка заказов.
        /// </summary>
        /// <returns>Оптимальный размер.</returns>
        private Double GetOptimalItemWidth()
        {
            Double width;

            if (WindowState == WindowState.Maximized)
                width = RenderSize.Width - 55;

            else
                width = Width - 55;

            return width;
        }
        #endregion

        #region Область: События закрытия окна.

        /// <summary>
        /// Событие нажатия на кнопку перехода на предыдущую страницу.
        /// <br/><br/>
        /// Program <b>always come back</b>.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void GoToPreviousPage_Click(Object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            Close();
        }

        /// <summary>
        /// Событие закрытия окна.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Closing(Object sender, System.ComponentModel.CancelEventArgs e) =>
                SessionData.GenerateNewOrder();
        #endregion
    }
}
