using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_VargasValeria.Models
{
    public class EstudianteCarrera
    {
        public int EstudianteId { get; set; }
        [ForeignKey("EstudianteId")]
        public Estudiante Estudiante { get; set; }

        public int CarreraId { get; set; }
        [ForeignKey("CarreraId")]
        public Carrera Carrera { get; set; }
    }
}
