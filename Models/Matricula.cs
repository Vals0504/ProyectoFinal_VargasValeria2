using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace ProyectoFinal_VargasValeria.Models

    {
        public enum EstadoMatricula
        {
            Activa,
            Cancelada,
            Finalizada
        }

        public class Matricula
        {
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "El estudiante es obligatorio")]
            public int EstudianteId { get; set; }

            [ForeignKey("EstudianteId")]
            public Estudiante Estudiante { get; set; }

            [Required(ErrorMessage = "El curso es obligatorio")]
            public int CursoId { get; set; }

            [ForeignKey("CursoId")]
            public Curso Curso { get; set; }


            [Required(ErrorMessage = "La fecha de matrícula es obligatoria")]
            [DataType(DataType.Date)]
            public DateTime FechaMatricula { get; set; } = DateTime.Now;

            [Required(ErrorMessage = "El estado es obligatorio")]
            public EstadoMatricula Estado { get; set; }
        }
    }

