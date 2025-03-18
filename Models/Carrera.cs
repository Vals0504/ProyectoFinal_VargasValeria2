using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_VargasValeria.Models
{
    public class Carrera
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la carrera es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre de la carrera no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        public ICollection<CursosCarreras> CursosCarreras { get; set; } = new List<CursosCarreras>();
        public ICollection<Estudiante> Estudiantes { get; set; } = new HashSet<Estudiante>();
    }
}
