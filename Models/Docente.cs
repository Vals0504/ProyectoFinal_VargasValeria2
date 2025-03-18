using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_VargasValeria.Models
{
    public class Docente
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "La identificación es oblicatoria")]
        [StringLength(20, ErrorMessage = "La longitud de la identificación no puede superar los 20 caracteres")]
        public string Identificacion { get; set; }


        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, ErrorMessage = "La longitud del nombre no puede superar los 50 caracteres")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "El correo es requerido")]
        [StringLength(50, ErrorMessage = "La longitud del correo no puede superar los 50 caracteres")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio")]
        [Phone(ErrorMessage = "El número de teléfono no es válido")]
        public string Telefono { get; set; }
        public ICollection<DocenteCursos> DocenteCursos { get; set; } = new HashSet<DocenteCursos>();
    }
}
