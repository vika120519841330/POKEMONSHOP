using POKEMONLIBRARY.Models;
using POKEMONSHOP.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POKEMONSHOP.Services
{
    public class PokemonService
    {
        private readonly IRepository rep;
        public PokemonService(IRepository rep)
        {
            this.rep = rep;
        }

        /// <summary>
        /// Метод для формирования ленты
        /// </summary>
        /// <returns>Кортеж, содержащий количество заказов покемона, сгруппированное по покупателю и дате заказа</returns>
        public Dictionary<string, Dictionary<DateTime, int>> GetLineAllOrdersSomeCustomers()
        {
            Dictionary<string, Dictionary<DateTime, int>> result = new Dictionary<string, Dictionary<DateTime, int>>();

            List<Customer> allCustom = this.rep?.Customers?.GetAllCustomers() ?? new List<Customer>();

            if (allCustom?.Count > 0)
            {
                foreach(Customer customer in allCustom)
                {
                    List<Order> orders = this.rep?.Orders?.GetAllOrders()?.Where(_ => _.CustomerId == customer.Id)?.ToList() ?? new List<Order>();

                    if(orders?.Count > 0)
                    {
                        Dictionary<DateTime, int> temp = orders.GroupBy(_ => _.DateOrder)
                                                               .Select(item => new
                                                                                     {
                                                                                         Date = item.Key,
                                                                                         Num = item.Count()
                                                                                     }
                                                               )
                                                               ?
                                                               .ToDictionary(a => a.Date, b => b.Num)
                                                               ;
                        if(!result.ContainsKey(customer.Name))
                        {
                            result.Add(customer.Name, temp);
                        }
                    }
                }
            }
            return result;
        }
    }
}
