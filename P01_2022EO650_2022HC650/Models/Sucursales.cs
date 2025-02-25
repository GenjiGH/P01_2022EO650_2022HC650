using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace P01_2022EO650_2022HC650.Models
{
    public class Sucursales
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSucursal { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(255)]
        public string Direccion { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefono { get; set; }

        [Required]
        public int IdAdministrador { get; set; }

        [Required]
        public int NumEspaciosDisponibles { get; set; } = 0;

        [ForeignKey("IdAdministrador")]
        public Usuarios Administrador { get; set; }
    }
}
