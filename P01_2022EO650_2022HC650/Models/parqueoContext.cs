using Microsoft.EntityFrameworkCore;

namespace P01_2022EO650_2022HC650.Models
{
    public class parqueoContext: DbContext
    {
        public parqueoContext(DbContextOptions<parqueoContext> options) : base(options)
        {

        }

        public DbSet<Sucursal> sucursal { get; set; }
        public DbSet<Usuario> usuario { get; set; }
        public DbSet<EspaciosParqueo> espaciosParqueos { get; set; }
        public DbSet<Reserva> reserva { get; set; }

    }
}
