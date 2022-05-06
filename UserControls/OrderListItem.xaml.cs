using System.Windows;
using System.Windows.Controls;
using _41_размер.Entities;

namespace _41_размер.UserControls
{
    /// <summary>
    /// Логика взаимодействия для OrderListItem.xaml
    /// </summary>
    public partial class OrderListItem : UserControl
    {
        /// <summary>
        /// Свойство с текущим заказом.
        /// </summary>
        public Order CurrentOrder { get; private set; }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="currentOrder">Текущий заказ.</param>
        public OrderListItem(Order currentOrder)
        {
            CurrentOrder = currentOrder;

            InitializeComponent();
            DataContext = CurrentOrder;

            InitializeVisibility();
            InitializeMainContextAction();
        }

        /// <summary>
        /// Функция инициализации видимости определенных элементов.
        /// </summary>
        private void InitializeVisibility()
        {
            // Проверка на то, что оставивший заказ — гость.
            if (CurrentOrder.User == null)
            {
                OrderConsumerInfo.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Функция инициализации описания основного действия контекстного меню.
        /// </summary>
        private void InitializeMainContextAction()
        {
            if (CurrentOrder?.Status?.StatusId == 1)
            {
                MainContextAction.Header = "Обозначить заказ как \"Выполненный\"";
            }

            else
            {
                MainContextAction.Header = "Обозначить заказ как \"Новый\"";
            }
        }
    }
}
