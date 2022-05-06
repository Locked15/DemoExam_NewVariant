using System;
using System.Linq;
using _41_размер.Entities;
using Bool = System.Boolean;

namespace _41_размер.Classes
{
    /// <summary>
    /// Класс, содержащий данные о текущей сессии.
    /// </summary>
    public class SessionData
    {
        #region Область: Свойства.

        /// <summary>
        /// Свойство с текущим пользователем.
        /// </summary>
        public static User CurrentUser { get; set; }

        /// <summary>
        /// Свойство с текущим заказом.
        /// </summary>
        public static Order CurrentOrder { get; set; }
        #endregion

        #region Область: Методы.

        /// <summary>
        /// Выполняет попытку входа в аккаунт.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="userPassword">Пароль.</param>
        /// <returns>Результат попытки входа.</returns>
        public static Bool TryToLoginInAccount(String userName, String userPassword)
        {
            User availableUser = TradeEntities.Instance.Users.Where(user =>
                                 user.UserLogin.Equals(userName) &&
                                 user.UserPassword.Equals(userPassword)).FirstOrDefault();

            // Для упрощения понимания кода все вынесено в отдельные блоки.
            if (availableUser != null)
            {
                CurrentOrder = (CurrentUser == availableUser && CurrentOrder != null) ? CurrentOrder : new Order()
                {
                    ConsumerId = availableUser.UserID,
                    User = availableUser,
                    OrderBeginDate = DateTime.Now,
                    OrderDeliveryDate = DateTime.Now.AddDays(3),
                    OrderStatus = 1,
                    Status = TradeEntities.Instance.Status.FirstOrDefault(),
                    TakeCode = new Random().Next(100, 1000)
                };
                CurrentUser = availableUser;

                return true;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Входит в систему в качестве гостя.
        /// </summary>
        public static void ContinueAsGuest()
        {
            CurrentUser = new User()
            {
                UserName = "Гость",
                UserRole = 4
            };
            GenerateNewOrder();
        }

        /// <summary>
        /// Генерирует новый заказ в процессе работы.
        /// <br/><br/>
        /// Не нужно использовать данный метод для генерации заказа при начале работы нового пользователя — это может привести к потере данных.<br/>
        /// (Единственное исключение — заказы гостей, их данные в любом случае не сохраняются).
        /// </summary>
        public static void GenerateNewOrder()
        {
            if (CurrentUser == null || CurrentUser.UserRole == 4)
            {
                CurrentOrder = new Order()
                {
                    OrderBeginDate = DateTime.Now,
                    OrderDeliveryDate = DateTime.Now.AddDays(3),
                    OrderStatus = 1,
                    Status = TradeEntities.Instance.Status.FirstOrDefault(),
                    TakeCode = new Random().Next(100, 1000)
                };
            }

            else
            {
                CurrentOrder = new Order()
                {
                    ConsumerId = CurrentUser.UserID,
                    User = CurrentUser,
                    OrderBeginDate = DateTime.Now,
                    OrderDeliveryDate = DateTime.Now.AddDays(3),
                    OrderStatus = 1,
                    Status = TradeEntities.Instance.Status.FirstOrDefault(),
                    TakeCode = new Random().Next(100, 1000)
                };
            }
        }
        #endregion
    }
}
