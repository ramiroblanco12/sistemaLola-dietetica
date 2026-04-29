using Proyecto_Lola.Models;
namespace Proyecto_Lola.ViewModels
{
    public class VentaIndexVM
    {
        public List<Venta> Ventas { get; set; } = new();
        public decimal TotalGeneral { get; set; }
        public decimal TotalEfectivo { get; set; }
        public decimal TotalDebito { get; set; }

        public string TipoTotalSeleccionado { get; set; } = "General";
    }
}
