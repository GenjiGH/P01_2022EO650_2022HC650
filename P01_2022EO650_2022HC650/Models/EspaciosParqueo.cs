using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace P01_2022EO650_2022HC650.Models
{
    public class EspaciosParqueo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEspacio { get; set; }

        [Required]
        public int IdSucursal { get; set; }

        [Required]
        public int NumeroEspacio { get; set; }

        [Required]
        [StringLength(100)]
        public string Ubicacion { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal CostoPorHora { get; set; }

        [Required]
        [StringLength(10)]
        public string Estado { get; set; } = "Disponible";

        [ForeignKey("IdSucursal")]
        public Sucursal Sucursal { get; set; }
    }
}
