using POKEMONLIBRARY.Configuration;
using POKEMONSHOP.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POKEMONSHOP.Services
{
    public class Repository : IRepository
    {
        private readonly IOrder orderServ;
        private readonly ICustomer customServ;

        public Repository (IOrder orderServ, ICustomer customServ)
        {
            this.orderServ = orderServ;
            this.customServ = customServ;
        }
        public IOrder Orders => this.orderServ;

        public ICustomer Customers => this.customServ;
    }
}
