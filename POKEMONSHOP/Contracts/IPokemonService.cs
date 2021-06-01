using POKEMONLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POKEMONSHOP.Contracts
{
    public interface IPokemonService
    {
        Task<Order> CreateOrder(Order order);
        List<Order> GetAllOrdersSomeCustomer(int idCustomer);

        List<Customer> GetAllCustomers();

        int GetNumberOrders(int idCustomer);
        Customer CreateCustomer(Customer customer);

        // async
        Task<List<Customer>> GetAllCustomers_async();
    }
}
