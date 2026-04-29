using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Lola.Data;
using Proyecto_Lola.Servicios;
using System.Net.Http.Json;
using Proyecto_Lola.Dtos.Printing;
using Proyecto_Lola.Models;


namespace Proyecto_Lola.Controllers
{
    [ApiController]
    [Route("api/impresion")]
    public class ImpresionController : Controller
    {
        private readonly LolaDbContext _db;
        private readonly ImpresoraTermica _impresora;

        public ImpresionController(
            LolaDbContext db,
            ImpresoraTermica impresora)
        {
            _db = db;
            _impresora = impresora;
        }

        // Endpoint de prueba
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new
            {
                ok = true,
                impresora = _impresora.NombreImpresora
            });
        }

        private readonly IHttpClientFactory _httpFactory;
        public ImpresionController(LolaDbContext db, ImpresoraTermica impresora, IHttpClientFactory httpFactory)
        {
            _db = db;
            _impresora = impresora;
            _httpFactory = httpFactory;
        }

        [HttpGet("cartel/{id:int}")]
        public async Task<IActionResult> DebugCartelGet(int id)
        {
            // Llamá internamente al mismo método de impresión (o repetí lógica)
            return Ok(new { ok = true, id });
        }

        [HttpPost("cartel/{id:int}")]
        public async Task<IActionResult> ImprimirCartel(int id)
        {
            var p = await _db.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null)
                return NotFound("Producto no encontrado.");

            // AJUSTÁ los nombres de propiedades según tu clase Producto
            var req = new PrintinLabelRequest
            {
                nombreProducto = p.Nombre,
                precio = p.PrecioFinal,
                Descripcion = p.Descripcion,
            };

            var client = _httpFactory.CreateClient("PrintAgent");

            // Si tu PrintAgent NO pide PrinterName (como lo dejamos), está perfecto.
            // Si todavía lo pidiera, habría que mandarlo, pero vos ya lo sacaste.
            var resp = await client.PostAsJsonAsync("/print/label", req);

            if (!resp.IsSuccessStatusCode)
            {
                var txt = await resp.Content.ReadAsStringAsync();
                return StatusCode((int)resp.StatusCode, $"PrintAgent error: {txt}");
            }

            return Ok(new { ok = true, producto = id });
        }



    }
}
