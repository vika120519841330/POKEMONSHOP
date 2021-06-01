using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using POKEMONLIBRARY.Models;

namespace POKEMONAPI
{
    public class ApiService
    {
        public HttpClient httpClient;

        public ApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Метод для получени\ списка всех покупателей, зарегистрированных в системе
        /// этот метод вызывает API-контроллера CustomersController
        /// </summary>
        /// <returns></returns>
        public async Task<List<Customer>> GetAllCustomers_async()
        {
            var response = await httpClient.GetAsync($"/api/Customers");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<List<Customer>>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });

            return result;
        }
    }
}
