namespace ProyectoFinal_VargasValeria.Models
{
    public class DocenteCursos
    {
        public int DocenteId { get; set; }
        public Docente Docente { get; set; }

        public int CursoId { get; set; }
        public Curso Curso { get; set; }
    }
}
