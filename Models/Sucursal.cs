namespace Proyecto_Lola.Models
{
    public class Sucursal
    {
        public int SucursalID { get; set; }
        public string Nombre { get; set; }

        public string Codigo { get; set; }

        public bool Activo { get; set; }

        public ICollection<ProductoSucursal> ProductosSucursal { get; set; } = new List<ProductoSucursal>();
    }
}
