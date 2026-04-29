namespace Proyecto_Lola.Models
{
    public class Stock
    {

        public int Id { get; set; }

        public int ProductoId { get; set; }

        public Producto producto { get; set; } = null!;

        public DateTime Fecha { get; set; }

        public decimal cantidad { get; set; }

        public string Tipo { get; set; } = null!;

        public string Motivo { get; set; } = null!;





    }
}
