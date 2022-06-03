using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Models
{
    public class Platillo
    {
        [Key]
        public int Id{get;set;}
        [Required]
        [Display(Name = "Nombre de platillo")]
        public string Nombre{get;set;}
        [Display(Name ="Categoria")]
        public int IdCategoria{get;set;}
        [ForeignKey("IdCategoria")]
        public Categoria Categoria {get;set;}
        [Required]
        [Display(Name = "Descripción del platillo")]
        public string Descripcion {get;set;}
        [Required]
        [Display(Name = "Precio del platillo")]
        public int precio {get;set;}

        [Display(Name = "Imagen del platillo")]
        public string? UrlImagen {get;set;}
    }
}