using Microsoft.EntityFrameworkCore;
using System.Linq;
using POKEMONLIBRARY.Models;

namespace POKEMONLIBRARY.Configuration
{
    public class PokemonDbContext : DbContext
    {
        public PokemonDbContext(DbContextOptions<PokemonDbContext> options)
            : base(options)
        { }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        // Покупатели
        public DbSet<Customer> Customers { get; set; }

        // Заказы
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Применить конфигурацию для покупателя
            modelBuilder.ApplyConfiguration<Customer>(new Customer_Configuration());

            // Применить конфигурацию для заказа
            modelBuilder.ApplyConfiguration<Order>(new Order_Configuration());

            base.OnModelCreating(modelBuilder);
        }
    }
}

