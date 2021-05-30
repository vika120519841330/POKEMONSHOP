using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace POKEMONSHOP.Data
{
    public class IdentityPokemonDbContext : IdentityDbContext
    {
        public IdentityPokemonDbContext(DbContextOptions<IdentityPokemonDbContext> options)
            : base(options)
        {
        }
    }
}
