namespace Proyecto_Lola.Models
{
    public class CierreCaja
    {

        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public decimal TotalVentasEfectivo { get; set; }
        public decimal TotalVentasTarjeta { get; set; }
        public decimal TotalVentasGastos { get; set; }
        public decimal TotalVentasEsperado { get; set; }
        public decimal TotalVentasContado { get; set; }
        public decimal Diferencia { get; set; }
    }
}
