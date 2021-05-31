using POKEMONLIBRARY.Models;
using POKEMONSHOP.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POKEMONSHOP.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IRepository rep;
        public PokemonService(IRepository rep)
        {
            this.rep = rep;
        }

        /// <summary>
        /// ����� ��� ���������� � �� ������ ����������
        /// </summary>
        /// <param name="customer">����������, ���������� ���������� � ��</param>
        /// <returns>����������, ����������� � ��, ���� ��� ������������ � ����� �� ������� email ����������</returns>
        public Customer CreateCustomer(Customer customer)
        {
            Customer res = new Customer();

            var foundCustomer = this.rep?.Customers?.GetAllCustomers()?.FirstOrDefault(_ => _.Email.Equals(customer?.Email, StringComparison.Ordinal)) ?? new Customer();
            if (!(foundCustomer.Id > 0))
            {
                res = this.rep?.Customers?.AddCustomer(customer) ?? new Customer();
            }
            else
            {
                res = foundCustomer;
            }

            return res;
        }
        /// <summary>
        /// ����� ��� ���������� � �� ������ ������
        /// </summary>
        /// <param name="order">�����, ���������� ���������� � ��</param>
        /// <returns>�����, ����������� � ��</returns>
        public Order CreateOrder(Order order) => this.rep?.Orders?.CreateOrder(order) ?? new Order();

        /// <summary>
        /// ����� ��� ������������ �����
        /// </summary>
        /// <returns>������, ���������� ���������� ������� ��������, ��������������� �� ���������� � ���� ������</returns>
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
                        //Dictionary<DateTime, int> temp = orders.GroupBy(_ => _.DateOrder)
                        //                                       .Select(item => new
                        //                                                             {
                        //                                                                 Date = item.Key,
                        //                                                                 Num = item.Count()
                        //                                                             }
                        //                                       )
                        //                                       ?
                        //                                       .ToDictionary(a => a.Date, b => b.Num)
                        //                                       ;

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
                        //if (!result.ContainsKey(customer.Name))
                        //{
                        //    result.Add(customer.Name, temp);
                        //}
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// ����� ��� ��������� ������ ����� ���������� ��������� �������� ����������� �� ��� �����
        /// </summary>
        /// <param name="idCustomer">������������� ����������/param>
        /// <returns>����� �������</returns>
        public int GetNumberOrders(int idCustomer) => this.rep?.Orders?.GetAllOrders().Where(_ => _.CustomerId == idCustomer)?.Count() ?? 0;
    }
}
