namespace Proyecto_Lola.Models
{
    public class Gastos
    {

        public int Id { get; set; }

        public DateTime FechaHora { get; set; }

        public decimal Monto { get; set; }

        public string Descripcion { get; set; } = null!;

    }
}
