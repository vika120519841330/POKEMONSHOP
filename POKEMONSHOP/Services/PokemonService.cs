using Microsoft.AspNetCore.Identity.UI.Services;
using POKEMONLIBRARY.Models;
using POKEMONSHOP.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POKEMONSHOP.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IRepository rep;
        private readonly IEmailSender emailSender;

        public PokemonService(IRepository rep, IEmailSender emailSender)
        {
            this.rep = rep;
            this.emailSender = emailSender;
        }

        /// <summary>
        /// Метод для добавления в БД нового покупателя
        /// </summary>
        /// <param name="customer">Покупатель, подлежащий добавлению в БД</param>
        /// <returns>Покупатель, добавленный в БД, либо уже существующий с таким же адресом email покупатель</returns>
        public Customer CreateCustomer(Customer customer)
        {
            Customer res = new Customer();

            var foundCustomer = this.rep?.Customers?.GetAllCustomers()?.FirstOrDefault(_ => _.Email.Equals(customer?.Email, StringComparison.Ordinal)) ?? new Customer();
            if (!(foundCustomer.Id > 0))
            {
                res = this.rep?.Customers?.AddCustomer(customer) ?? new Customer();
            }
            else
            {
                res = foundCustomer;
            }

            return res;
        }
        /// <summary>
        /// Метод для добавления в БД нового заказа
        /// </summary>
        /// <param name="order">Заказ, подлежащий добавлению в БД</param>
        /// <returns>Заказ, добавленный в БД</returns>
        public async Task<Order> CreateOrder(Order order)
        {
            Order res = this.rep?.Orders?.CreateOrder(order) ?? new Order();
            string email = this.rep?.Customers?.GetCustomer(order.CustomerId)?.Email ?? string.Empty;
            string title = $"Информация о заказе № {order.Id} от {order.DateOrder.Day}.{order.DateOrder.Month}.{order.DateOrder.Year}г.";
            string message = $"Заказ № {order.Id} успешно принят к обработке {order.DateOrder.Day}.{order.DateOrder.Month}.{order.DateOrder.Year} {order.DateOrder.Hour}:{order.DateOrder.Minute}";

            await this.SendorderInfoToToEMail(email, title, message);

            return res;
        }

        /// <summary>
        /// Вспомогательный метод для отправки сведений о заказе покемона на почту покупателя
        /// </summary>
        /// <param name="email">Эл.почта</param>
        /// <param name="title">Заголовок письма</param>
        /// <param name="message">Тело письма</param>
        /// <returns></returns>
        private async Task SendorderInfoToToEMail(string email, string title, string message) =>  await this.emailSender.SendEmailAsync(email, title, message);

        /// <summary>
        /// Метод для получения коллекции всех заказов заданного покупателя
        /// </summary>
        /// <returns></returns>
        public List<Order> GetAllOrdersSomeCustomer(int idCustomer) => this.rep?.Orders?.GetAllOrders()?.Where(_ => _.CustomerId == idCustomer)?.OrderByDescending(_ => _.DateOrder)?.ToList() ?? new List<Order>();

        /// <summary>
        /// Метод для получения общего числа заказанных покемонов заданным покупателем за все время
        /// </summary>
        /// <param name="idCustomer">Идентификатор покупателя/param>
        /// <returns>Число заказов</returns>
        public int GetNumberOrders(int idCustomer) => this.rep?.Orders?.GetAllOrders().Where(_ => _.CustomerId == idCustomer)?.Count() ?? 0;

        /// <summary>
        /// Метод для получения коллекции всех зарегистр-х в БД покупателей
        /// </summary>
        /// <returns></returns>
        public List<Customer> GetAllCustomers() => this.rep?.Customers?.GetAllCustomers() ?? new List<Customer>();
    }
}
