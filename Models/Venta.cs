namespace Proyecto_Lola.Models
{
    public class Venta
    {

        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public Decimal Total { get; set; }

        public string MedioPago { get; set; } = null!;

        public List<DetalleVenta> Detalles { get; set; }





    }
}
