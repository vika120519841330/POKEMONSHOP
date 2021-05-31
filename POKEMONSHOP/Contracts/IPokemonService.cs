using POKEMONLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POKEMONSHOP.Contracts
{
    public interface IPokemonService
    {
        Dictionary<string, Dictionary<DateTime, int>> GetLineAllOrdersSomeCustomers();
        Task<Order> CreateOrder(Order order);
        int GetNumberOrders(int idCustomer);
        Customer CreateCustomer(Customer customer);
    }
}
