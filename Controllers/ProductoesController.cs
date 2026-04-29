using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.EntityFrameworkCore;
using Proyecto_Lola.Data;
using Proyecto_Lola.Models;


namespace Proyecto_Lola.Controllers
{
    public class ProductoesController : Controller
    {
        private readonly Decimal _margenGeneral;
        private readonly LolaDbContext _context;

        public ProductoesController(LolaDbContext context, IConfiguration config)
        {
            _margenGeneral = config.GetValue<decimal>("MargenGeneral");
            _context = context;
        }

        // GET: Productoes
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Categorias = new SelectList(await _context.Categorias.AsNoTracking().ToListAsync(),
                "Id", "Descripcion");
            ViewBag.Proveedores= new SelectList(await _context.Proveedores.AsNoTracking().ToListAsync(),
             "Id", "Nombre");
            var lolaDbContext = _context.Productos.Include(p => p.Categoria);
            return View(productos);
        }



        public async Task<IActionResult> EditarModal(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();
            return PartialView("_ProductoModal", producto);
        }

        //private (string codigoInterno, decimal precioTicket) ParseCodigoBalanza(string codigo)
        //{

        //    if (codigo.Length != 13)
        //        throw new ArgumentException("Codigo de balanza invalido.");

        //    var prefijo = codigo.Substring(0, 2);

        //    var codigoInternoBruto = codigo.Substring(2, 4);
        //    var codigoInterno = int.Parse(codigoInternoBruto).ToString();

        //    var precioStr = codigo.Substring(6, 4);
        //    var precioBase = int.Parse(precioStr);

        //    decimal precioTicket = precioBase * 100m;

        //    return (codigoInterno, precioTicket);
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarDesdeModal(Producto model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ProductoModal", model);
            }

            var producto = await _context.Productos.FindAsync(model.Id);
            if (producto == null) return NotFound();

            var porcentaje = model.PorcentajeIndividual;

            decimal margen = (porcentaje ?? _margenGeneral);
            producto.PrecioFinal = producto.Costo + (producto.Costo * margen / 100m);

            producto.Nombre = model.Nombre;
            producto.Descripcion = model.Descripcion;
            producto.CodigoBarras = model.CodigoBarras;
            producto.Costo = model.Costo;
            producto.PorcentajeIndividual = model.PorcentajeIndividual;
            producto.CategoriaId = model.CategoriaId;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult DatosEtiqueta(int id)
        {
            var p = _context.Productos.Find(id);

            if (p == null) return NotFound();

            return Json(new
            {
                nombre = p.Nombre,
                precio = p.PrecioFinal,
                descripcion = p.Descripcion,
                codigo = p.CodigoBarras
            });
        }

        public async Task<IActionResult> BuscarPorCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return BadRequest("Código vacío");


            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.CodigoBarras == codigo);


            if (producto == null)
                return NotFound();


            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Descripcion", producto.CategoriaId);

            return PartialView("_ProductoModal", producto);
        }
        [HttpGet]
        public async Task<IActionResult> BuscarPorCodigoJson(string codigo)
        {
            try
            {
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.CodigoBarras == codigo);

                if (producto == null)
                    return NotFound();

                var precioFinal = producto.PrecioFinal;
                var costo = producto.Costo;


                return Json(new
                {
                    id = producto.Id,
                    nombre = producto.Nombre,
                    precio = producto.PrecioFinal,
                    descripcion = producto.Descripcion,
                    Categoria = producto.CategoriaId,

                });


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en BuscarPorCodigoJson" + ex.Message);

                return StatusCode(500, new
                {
                    error = "Error interno al procesar la busqueda",
                    detalle = ex.Message
                });

            }

        }

        // GET: Productoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(x => x.Proveedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productoes/Create
        public IActionResult Create()
        {
            ViewBag.CategoriasId = new SelectList(_context.Categorias, "Id", "Descripcion");
            ViewBag.ProveedorId = new SelectList(_context.Proveedores, "Id", "Nombre");

            return View();
        }

        // POST: Productoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                ViewBag.CategoriasId = new SelectList(_context.Categorias, "Id", "Descripcion");
                ViewBag.ProveedorId = new SelectList(_context.Proveedores, "Id", "Nombre");
                decimal margen = (producto.PorcentajeIndividual ?? _margenGeneral);
                producto.PrecioFinal = producto.Costo + (producto.Costo * margen / 100m);
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewBag.CategoriasId = new SelectList(_context.Categorias, "Id", "Descripcion");
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,CodigoBarras,CategoriaId,Costo,PrecioFinal,StockActual,StockMinimo,UnidadMedida,PorcentajeIndividual")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ModelState.Remove("PrecioFinal");
                try
                {
                    decimal margen = (producto.PorcentajeIndividual ?? (int)_margenGeneral);
                    producto.PrecioFinal = producto.Costo + (producto.Costo * margen / 100);

                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
