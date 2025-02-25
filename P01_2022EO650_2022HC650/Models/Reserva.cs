using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace P01_2022EO650_2022HC650.Models
{
    public class Reserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdReserva { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public int IdEspacio { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime FechaReserva { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan HoraReserva { get; set; }

        [Required]
        public int CantidadHoras { get; set; }

        [Required]
        [StringLength(10)]
        public string Estado { get; set; } = "Activa";

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }

        [ForeignKey("IdEspacio")]
        public EspaciosParqueo EspacioParqueo { get; set; }
    }
}
