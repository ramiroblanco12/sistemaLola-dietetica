using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Lola.Data;
using Proyecto_Lola.Models;

namespace Proyecto_Lola.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly LolaDbContext _context;

        public ProveedoresController(LolaDbContext context)
        {
            _context = context;
        }
        // GET: ProveedoresController
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proveedores.ToListAsync());
        }


        [HttpPost]
        public IActionResult CambiarEstado( int id )
        {
            var proveedor = _context.Proveedores.Find(id);
            if (proveedor == null) return NotFound();
            proveedor.Activo = !proveedor.Activo;
            return RedirectToAction(nameof(Index));
        }



        // GET: ProveedoresController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProveedoresController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProveedoresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Proveedores Proveedor)
        {
            try
            {
                _context.Add(Proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProveedoresController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProveedoresController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProveedoresController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProveedoresController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
