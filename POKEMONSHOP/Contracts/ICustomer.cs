using POKEMONLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POKEMONSHOP.Contracts
{
    public interface ICustomer
    {
        Customer GetCustomer(int id);
        List<Customer> GetAllCustomers();
    }
}
