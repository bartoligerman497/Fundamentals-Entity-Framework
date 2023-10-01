using Microsoft.EntityFrameworkCore;
using ProyectoEF.Models;

namespace ProyectoEF
{
    public class TareasContext : DbContext
    {
        // Set de datos, tabla
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public TareasContext(DbContextOptions<TareasContext> options) : base(options) { }

        // Creando modelo de categoría con Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            List<Categoria> categoriasInit = new List<Categoria>();
            categoriasInit.Add(new Categoria { CategoriaId = Guid.Parse("2bbe0713-85a7-4951-ab3b-bf4b5c1fd736"), Nombre="Actividades pendientes", Peso=20 });
            categoriasInit.Add(new Categoria { CategoriaId = Guid.Parse("2bbe0713-85a7-4951-ab3b-bf4b5c1fd702"), Nombre="Actividades personales", Peso=50 });
            modelBuilder.Entity<Categoria>(categoria =>
            {
                categoria.ToTable("Categoria");
                categoria.HasKey(p => p.CategoriaId);
                categoria.Property(p=> p.Nombre).IsRequired().HasMaxLength(150);
                categoria.Property(p => p.Descripcion).IsRequired(false);
                categoria.Property(p => p.Peso);
                categoria.HasData(categoriasInit);
            });

            List<Tarea> tareasInit = new List<Tarea>();
            tareasInit.Add(new Tarea { TareaId = Guid.Parse("42c1b2a3-aa85-4b8f-a791-7ef8308caebb"), CategoriaId = Guid.Parse("2bbe0713-85a7-4951-ab3b-bf4b5c1fd736"), PrioridadTarea = Prioridad.Media, Titulo = "Pago de servicios publicos", FechaCreacion = DateTime.Now });
            tareasInit.Add(new Tarea { TareaId = Guid.Parse("42c1b2a3-aa85-4b8f-a791-7ef8308cae11"), CategoriaId = Guid.Parse("2bbe0713-85a7-4951-ab3b-bf4b5c1fd702"), PrioridadTarea = Prioridad.Baja, Titulo = "Terminar de ver peliculas en Netflix", FechaCreacion = DateTime.Now });
            modelBuilder.Entity<Tarea>(tarea =>
            {
                tarea.ToTable("Tarea");
                tarea.HasKey(p => p.TareaId);
                tarea.HasOne(p => p.Categoria).WithMany(p => p.Tareas).HasForeignKey(p => p.CategoriaId);
                tarea.Property(p=>p.Titulo).HasMaxLength(200);
                tarea.Property(p=>p.Descripcion).HasMaxLength(200).IsRequired(false);
                tarea.Property(p => p.PrioridadTarea);
                tarea.Property(p => p.FechaCreacion);
                tarea.Ignore(p => p.Resumen);
                tarea.HasData(tareasInit);

            });
        }
    }
}
