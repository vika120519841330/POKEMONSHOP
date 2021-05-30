using POKEMONLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POKEMONSHOP.Contracts
{
    public interface IOrder
    {
        List<Order> GetAllOrders();
        Order CreateOrder(Order order);
    }
}
