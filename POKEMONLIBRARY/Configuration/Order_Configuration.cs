using Microsoft.EntityFrameworkCore;
using POKEMONLIBRARY.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POKEMONLIBRARY.Configuration
{
    public class Order_Configuration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                   .ToTable("Order")
                   .HasKey(_ => _.Id)
                   ;
            builder
                   .Property(_ => _.Id)
                   .ValueGeneratedOnAdd()
                   ;
            builder
                   .HasOne(_ => _.Customer)
                   .WithMany(_ => _.Orders)
                   .HasForeignKey(_ => _.CustomerId)
                   .OnDelete(DeleteBehavior.NoAction)
                   ;

            builder.HasComment(nameof(Order));
        }
    }
}
