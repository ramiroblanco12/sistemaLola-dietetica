using System.ComponentModel.DataAnnotations;

namespace Proyecto_Lola.Models
{
    public class Proveedores
    {
        public int Id { get; set; }

        public string  Nombre { get; set; }

        public string? Mail { get; set; }

        public string? NumeroTelefono { get; set; }
        public string? NumeroTelefono2 { get; set; }
        public string? NumeroTelefono3 { get; set; }
        [StringLength(250)]
        public string Anotacion { get; set; }
        public bool Activo { get; set; } = true;

        public ICollection<Producto> Productos { get; set; }
    }
}
