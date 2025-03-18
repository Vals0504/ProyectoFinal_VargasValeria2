namespace ProyectoFinal_VargasValeria.Models
{
    public class CursosCarreras
    {
        public int CursoId { get; set; }
        public Curso Curso { get; set; }

        public int CarreraId { get; set; }
        public Carrera Carrera { get; set; }
    }
}
