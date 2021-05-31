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
    /// Класс, представляющий собой заказ покемона авторизованным / неавторизованным покупателем
    /// </summary>
    [Table("Order")]
    public class Order: IId
    {
        [Key]
        public int Id { get; set; }

        // Ссылка на покупателя
        [Required]
        [ForeignKey(nameof(Customer))]
        [Display(Name = "Покупатель")]
        public int CustomerId { get; set; }

        [Display(Name = "Покупатель")]
        public virtual Customer Customer { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Дата и время заказа")]
        public DateTime DateOrder { get; set; } = DateTime.Now;
    }
}
