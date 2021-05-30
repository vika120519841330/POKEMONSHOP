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
    public class Customer_Configuration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
                   .ToTable("Customer")
                   .HasKey(_ => _.Id)
                   ;
            ;
            builder
                   .Property(_ => _.Id)
                   .ValueGeneratedOnAdd()
                   ;
            builder
                   .Property(_ => _.Name)
                   .IsRequired()
                   ;

            builder
                   .Property(_ => _.PhoneNumber)
                   .IsRequired()
                   ;

            builder.HasComment(nameof(Customer));
        }
    }
}
