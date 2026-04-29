namespace Proyecto_Lola.Models
{
    public class ProductoSucursal
    {

        public int ProductoSucursalId { get; set; }

        public int ProductoId { get; set; }

        public Producto producto { get; set; }

        public int  SucursalId { get; set; }

        public Sucursal sucursal { get; set; }

        public bool Activo { get; set; } = true;

        public decimal  precioVenta { get; set; }

        public DateTime FechaActivacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;


    }
}
