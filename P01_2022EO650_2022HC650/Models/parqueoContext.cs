using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace P01_2022EO650_2022HC650.Models
{
    public class parqueoContext: DbContext
    {
        public parqueoContext(DbContextOptions<parqueoContext> options) : base(options)
        {

        }

        public DbSet<Sucursales> Sucursales { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<EspaciosParqueo> EspaciosParqueo { get; set; }
        public DbSet<Reservas> Reservas { get; set; }

    }
}
