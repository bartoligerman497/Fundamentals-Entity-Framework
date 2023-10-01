﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProyectoEF.Models
{
    public class Categoria
    {
        //[Key]
        public Guid CategoriaId { get; set; }
        //[Required]
        //[MaxLength(150)]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Peso { get; set; } // Esfuerzo, impacto
        [JsonIgnore]
        public virtual ICollection<Tarea> Tareas { get; set; }
    }
}
