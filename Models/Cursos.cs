using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_VargasValeria.Models
{
    public class Curso
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del curso es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre del curso no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los créditos son obligatorios")]
        [Range(1, 10, ErrorMessage = "Los créditos deben estar entre 1 y 10")]
        public int Creditos { get; set; }

        public ICollection<Matricula> Matriculas { get; set; } = new HashSet<Matricula>();
        public ICollection<DocenteCursos> DocenteCursos { get; set; } = new HashSet<DocenteCursos>();

       
        public ICollection<CursosCarreras> CursosCarreras { get; set; } = new List<CursosCarreras>();
    }
}
