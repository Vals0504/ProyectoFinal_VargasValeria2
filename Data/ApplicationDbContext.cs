using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VargasValeria.Models;

namespace ProyectoFinal_VargasValeria.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Entidades personalizadas
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Docente> Docentes { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<DocenteCursos> DocenteCursos { get; set; }
        public DbSet<CursosCarreras> CursosCarreras { get; set; }
        public DbSet<Sede> Sedes { get; set; }
        public DbSet<CarreraSede> CarrerasSedes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Importante para Identity

            // Relación muchos a muchos con Matricula
            modelBuilder.Entity<Matricula>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Estudiante)
                .WithMany(e => e.Matriculas)
                .HasForeignKey(m => m.EstudianteId);

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Curso)
                .WithMany(c => c.Matriculas)
                .HasForeignKey(m => m.CursoId);

            // Relación muchos a muchos Docente-Cursos
            modelBuilder.Entity<DocenteCursos>()
                .HasKey(dc => new { dc.DocenteId, dc.CursoId });

            modelBuilder.Entity<DocenteCursos>()
                .HasOne(dc => dc.Docente)
                .WithMany(d => d.DocenteCursos)
                .HasForeignKey(dc => dc.DocenteId);

            modelBuilder.Entity<DocenteCursos>()
                .HasOne(dc => dc.Curso)
                .WithMany(c => c.DocenteCursos)
                .HasForeignKey(dc => dc.CursoId);

            // Relación muchos a muchos Cursos-Carreras
            modelBuilder.Entity<CursosCarreras>()
                .HasKey(cc => new { cc.CursoId, cc.CarreraId });

            modelBuilder.Entity<CursosCarreras>()
                .HasOne(cc => cc.Curso)
                .WithMany(c => c.CursosCarreras)
                .HasForeignKey(cc => cc.CursoId);

            modelBuilder.Entity<CursosCarreras>()
                .HasOne(cc => cc.Carrera)
                .WithMany(c => c.CursosCarreras)
                .HasForeignKey(cc => cc.CarreraId);

            modelBuilder.Entity<CarreraSede>()
                .HasKey(cs => new { cs.CarreraId, cs.SedeId });

            modelBuilder.Entity<CarreraSede>()
                .HasOne(cs => cs.Carrera)
                .WithMany(c => c.CarrerasSedes)
                .HasForeignKey(cs => cs.CarreraId);

            modelBuilder.Entity<CarreraSede>()
                .HasOne(cs => cs.Sede)
                .WithMany(s => s.CarrerasSedes)
                .HasForeignKey(cs => cs.SedeId);

        }
    }
}
