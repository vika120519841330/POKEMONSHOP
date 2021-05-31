using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IEmailSender emailSender;

        public PokemonService(IRepository rep, IEmailSender emailSender)
        {
            this.rep = rep;
            this.emailSender = emailSender;
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
        public async Task<Order> CreateOrder(Order order)
        {
            Order res = this.rep?.Orders?.CreateOrder(order) ?? new Order();
            string email = this.rep?.Customers?.GetCustomer(order.CustomerId)?.Email ?? string.Empty;
            string title = $"���������� � ������ � {order.Id} �� {order.DateOrder.Day}.{order.DateOrder.Month}.{order.DateOrder.Year}�.";
            string message = $"����� � {order.Id} ������� ������ � ��������� {order.DateOrder.Day}.{order.DateOrder.Month}.{order.DateOrder.Year} {order.DateOrder.Hour}:{order.DateOrder.Minute}";

            await this.SendorderInfoToToEMail(email, title, message);

            return res;
        }

        /// <summary>
        /// ��������������� ����� ��� �������� �������� � ������ �������� �� ����� ����������
        /// </summary>
        /// <param name="email">��.�����</param>
        /// <param name="title">��������� ������</param>
        /// <param name="message">���� ������</param>
        /// <returns></returns>
        private async Task SendorderInfoToToEMail(string email, string title, string message) =>  await this.emailSender.SendEmailAsync(email, title, message);

        /// <summary>
        /// ����� ��� ��������� ��������� ���� ������� ��������� ����������
        /// </summary>
        /// <returns></returns>
        public List<Order> GetAllOrdersSomeCustomer(int idCustomer) => this.rep?.Orders?.GetAllOrders()?.Where(_ => _.CustomerId == idCustomer)?.OrderByDescending(_ => _.DateOrder)?.ToList() ?? new List<Order>();

        /// <summary>
        /// ����� ��� ��������� ������ ����� ���������� ��������� �������� ����������� �� ��� �����
        /// </summary>
        /// <param name="idCustomer">������������� ����������/param>
        /// <returns>����� �������</returns>
        public int GetNumberOrders(int idCustomer) => this.rep?.Orders?.GetAllOrders().Where(_ => _.CustomerId == idCustomer)?.Count() ?? 0;

        /// <summary>
        /// ����� ��� ��������� ��������� ���� ���������-� � �� �����������
        /// </summary>
        /// <returns></returns>
        public List<Customer> GetAllCustomers() => this.rep?.Customers?.GetAllCustomers() ?? new List<Customer>();
    }
}
