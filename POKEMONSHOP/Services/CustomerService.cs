using Microsoft.EntityFrameworkCore;
using POKEMONLIBRARY.Configuration;
using POKEMONLIBRARY.Models;
using POKEMONSHOP.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POKEMONSHOP.Services
{
    public class CustomerService : ICustomer
    {
        private readonly PokemonDbContext context;
        public CustomerService(PokemonDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Метод для получения всех покупателей зарег-х в БД в качестве таковых
        /// </summary>
        /// <returns>Коллекция покупателей</returns>
        public List<Customer> GetAllCustomers() => this.context.Customers?.ToList() ?? new List<Customer>();

        /// <summary>
        /// Метод для получения покупателя по его идентификационному номеру
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Найденный покупатель</returns>
        public Customer GetCustomer(int id) => this.context.Customers?.FirstOrDefault(_ => _.Id == id) ?? new Customer();

        /// <summary>
        /// Метод для сохранения в БД нового покупателя
        /// </summary>
        /// <param name="item">Покупатель, подлежащий сохранению в БД</param>
        /// <returns>Покупатель, сохраненный в БД</returns>
        public Customer AddCustomer(Customer item)
        {
            var temp = this.context.Customers?.Add(item);
            context.SaveChanges();
            item = temp?.Entity ?? new Customer();
            return item;
        }

        /// <summary>
        /// Метод для получения всех покупателей зарег-х в БД в качестве таковых
        /// </summary>
        /// <returns>Коллекция покупателей</returns>
        public async Task<List<Customer>> GetAllCustomers_async() => await this.context.Customers?.ToListAsync();
    }
}
