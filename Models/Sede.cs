using System.ComponentModel.DataAnnotations;
namespace ProyectoFinal_VargasValeria.Models
{
    public class Sede
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public ICollection<CarreraSede> CarrerasSedes { get; set; } = new List<CarreraSede>();
    }

}
