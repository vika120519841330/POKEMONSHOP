using POKEMONLIBRARY.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POKEMONLIBRARY.Models
{
    /// <summary>
    /// Класс, представляющий собой авторизованного / неавторизованного покупателя
    /// </summary>
    [Table("Customer")]
    public class Customer : IId
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Адрес электронной почты")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Недопустимое значение формата адреса эл.почты")]
        [Required(ErrorMessage = "Адрес электронной почты обязателен для заполнения")]
        public string Email { get; set; } = string.Empty;

        public bool Equals(Customer other) => string.Equals(this.Email, other.Email, StringComparison.Ordinal);

        [Display(Name = "Заказы, сделанные покупателем")]
        public virtual List<Order> Orders { get; set; }
    }
}
