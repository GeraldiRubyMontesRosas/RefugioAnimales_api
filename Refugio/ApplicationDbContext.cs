using Microsoft.EntityFrameworkCore;
using Refugio.Entities;
using System.Text.RegularExpressions;

namespace Refugio
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Especie> Especies { get; set; }
        public DbSet<Tamaño> Tamaños { get; set; }
    }
}
