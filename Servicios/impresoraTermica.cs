using System;

namespace Proyecto_Lola.Servicios
{
    public class ImpresoraTermica
    {
        public string NombreImpresora { get; }

        public ImpresoraTermica(string nombreImpresora)
        {
            if (string.IsNullOrWhiteSpace(nombreImpresora))
                throw new ArgumentException("El nombre de la impresora no puede estar vacío.");

            NombreImpresora = nombreImpresora;
        }
    }
}
