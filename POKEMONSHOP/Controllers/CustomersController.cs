using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POKEMONSHOP.Contracts;
using POKEMONLIBRARY.Models;

namespace POKEMONSHOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : Controller
    {
        private readonly IPokemonService service;

        public CustomersController(IPokemonService service)
        {
            this.service = service;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersList()
        {
            return await this.service.GetAllCustomers_async();
        }
    }
}
