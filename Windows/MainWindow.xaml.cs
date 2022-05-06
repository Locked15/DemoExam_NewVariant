using System;
using System.Linq;
using System.Windows;
using System.Threading;
using _41_размер.Classes;

namespace _41_размер.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Область: Поля и свойства.

        /// <summary>
        /// Количество ошибок при входе.
        /// </summary>
        private Int32 errorCount = 0;

        /// <summary>
        /// Текущее введенное имя пользователя.
        /// </summary>
        public String UserName { get; set; } = default;

        /// <summary>
        /// Текущий введенный пароль пользователя.
        /// </summary>
        public String UserPassword { get; set; } = default;
        #endregion

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        #region Область: События.

        /// <summary>
        /// Событие изменения введенного пароля в "скрытом" поле.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserPassword_PasswordBox_PasswordChanged(Object sender, RoutedEventArgs e)
        {
            UserPassword = UserPassword_PasswordBox.Password;
        }

        /// <summary>
        /// Событие измененного пароля в "открытом" поле.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserPassword_TextBox_TextChanged(Object sender, RoutedEventArgs e)
        {
            UserPassword = UserPassword_TextBox.Text;
        }

        /// <summary>
        /// Событие переключения видимости полей для ввода пароля.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ShowPassword_Click(Object sender, RoutedEventArgs e)
        {
            if (UserPassword_PasswordBox.Visibility == Visibility.Hidden)
            {
                UserPassword_PasswordBox.Password = UserPassword;

                UserPassword_PasswordBox.Visibility = Visibility.Visible;
                UserPassword_TextBox.Visibility = Visibility.Hidden;
            }

            else
            {
                UserPassword_TextBox.Text = UserPassword;

                UserPassword_PasswordBox.Visibility = Visibility.Hidden;
                UserPassword_TextBox.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Отслеживает нажатия клавиш в текущем окне.
        /// <br/>
        /// Позволяет перехватить нажатие клавиши "Enter" и перенаправить её на событие входа в аккаунт.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_KeyUp(Object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                EnterInAccount_Click(sender, default);
        }

        /// <summary>
        /// Событие продолжения использования программы в качестве гостя.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ContinueAsGuest_Click(Object sender, RoutedEventArgs e)
        {
            SessionData.ContinueAsGuest();
            OpenProductsWindowAfterLogin();
        }

        /// <summary>
        /// Событие попытки входа в аккаунт по указанным данным.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void EnterInAccount_Click(Object sender, RoutedEventArgs e)
        {
            if (!SessionData.TryToLoginInAccount(UserName, UserPassword))
            {
                ++errorCount;

                if (errorCount < 3)
                {
                    MessageBox.Show("Указанный аккаунт не найден.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {
                    MessageBox.Show($"Слишком много ({errorCount}) попыток входа. Блокировка...", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Title = "!БЛОКИРОВКА!";

                    Thread.Sleep(10000);

                    Title = "Вход в систему";
                }
            }

            else
            {
                OpenProductsWindowAfterLogin();
            }
        }
        #endregion

        /// <summary>
        /// Открывает окно со списком продуктов и закрывает текущее, после успешной авторизации.
        /// </summary>
        private void OpenProductsWindowAfterLogin()
        {
            var list = Entities.TradeEntities.Instance.Orders.ToList();

            ProductsWindow newWindow = new ProductsWindow();

            newWindow.Show();
            Close();
        }
    }
}
