using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal_VargasValeria.Models
{
    public class Administrador
    {
        public enum RolAdministrador
        {
            Administrador,
            Estudiante,
            Docente
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El departamento es obligatorio")]
        [StringLength(100, ErrorMessage = "El departamento no puede exceder los 100 caracteres")]
        public string Departamento { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        public RolAdministrador Rol { get; set; }
    }
}
