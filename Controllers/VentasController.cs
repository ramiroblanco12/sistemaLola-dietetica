using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Lola.Data;
using Proyecto_Lola.Models;
using Proyecto_Lola.ViewModels;

namespace Proyecto_Lola.Controllers
{
    public class VentasController : Controller
    {
        private readonly LolaDbContext _context;

        public VentasController(LolaDbContext context)
        {
            _context = context;
        }

        public IActionResult PuntoDeVenta()
        {
            return View();
        }
        
        public class VentaRequest
        {
            public decimal Total { get; set; }
            public String FormaPago { get; set; } = "Efectivo";
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarVenta([FromBody]VentaRequest request)
        {
            if (request == null || request.Total <= 0)
            {
                return BadRequest("Total Invalido.");
            }
                var venta = new Venta
                {
                    Fecha = DateTime.Now,
                    Total = request.Total,
                    MedioPago = request.FormaPago
                };
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();
                
                return Ok(new {ventaId= venta.Id});
            
        }







        // GET: Ventas
        public async Task<IActionResult> Index(
            string tipoTotal = "General",
            DateTime? desde = null,
            DateTime? hasta = null)
        {
            var query = _context.Ventas
                .AsNoTracking()
                .AsQueryable();

            // Desde: incluye todo el día
            if (desde.HasValue)
                query = query.Where(v => v.Fecha >= desde.Value.Date);

            // Hasta: incluye todo el día (clave AddDays(1))
            if (hasta.HasValue)
                query = query.Where(v => v.Fecha < hasta.Value.Date.AddDays(1));

            var ventas = await query
                .OrderByDescending(x => x.Fecha)
                .ToListAsync();

            var vm = new VentaIndexVM
            {
                Ventas = ventas,
                TotalGeneral = ventas.Sum(v => v.Total),
                TotalEfectivo = ventas.Where(v => v.MedioPago == "Efectivo").Sum(v => v.Total),
                TotalDebito = ventas.Where(v => v.MedioPago == "Debito").Sum(v => v.Total),
                TipoTotalSeleccionado = tipoTotal
            };

            // Para que el input date quede cargado al recargar
            ViewBag.Desde = desde?.ToString("yyyy-MM-dd");
            ViewBag.Hasta = hasta?.ToString("yyyy-MM-dd");

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> DatosTicket(int id)
        {
            var venta = await _context.Ventas
                .AsNoTracking()
                .Include(v => v.Detalles)              // ajustá el nombre: Detalles / Items
                .ThenInclude(d => d.producto.Nombre)          // si querés el nombre del producto
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null) return NotFound();

            var dto = new
            {
                id = venta.Id,
                fecha = venta.Fecha.ToString("yyyy-MM-dd HH:mm"),
                medioPago = venta.MedioPago,
                total = venta.Total,
                // si tenés vuelto, descuento, etc. lo agregás acá
                items = venta.Detalles.Select(d => new {
                    nombre = d.producto.Nombre,
                    cantidad = d.Cantidad,
                    precioUnit = d.PrecioUnitario,
                    subtotal = d.Subtotal
                }).ToList()
            };

            return Json(dto);
        }


        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Total,MedioPago")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venta);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            return View(venta);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Total,MedioPago")] Venta venta)
        {
            if (id != venta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.Id == id);
        }
    }
}
