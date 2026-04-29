using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Reflection.Metadata.Ecma335;

namespace Proyecto_Lola.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public string? CodigoBarras { get; set; }


        public bool?  EsDeBalanza { get; set; }

        public string? CodigoBalanza { get; set; }

        [ValidateNever]
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; } = null!;
        public int? ProveedorId { get; set; }
        public Proveedores? Proveedor { get; set; }

        public decimal? PorcentajeIndividual { get; set; }
        public decimal Costo { get; set; }

        public decimal PrecioFinal { get; set; }

        public int? StockActual { get; set; }

        public int? StockMinimo { get; set; }

        public string? UnidadMedida { get; set; } = "unidad";

    }
}
