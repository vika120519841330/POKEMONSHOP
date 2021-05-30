using POKEMONLIBRARY.Configuration;
using POKEMONLIBRARY.Models;
using POKEMONSHOP.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POKEMONSHOP.Services
{
    public class OrderService : IOrder
    {
        private readonly PokemonDbContext context;
        public OrderService(PokemonDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Метод для создания нового заказа
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Order CreateOrder(Order order)
        {
            var temp = this.context.Add(order);
            context.SaveChanges();
            order = temp.Entity; 
            return order;
        }

        /// <summary>
        /// Метод для получения коллекции всех заказов, зарегистрированных в БД
        /// </summary>
        /// <returns></returns>
        public List<Order> GetAllOrders() => this.context.Orders?.ToList() ?? new List<Order>();
    }
}
