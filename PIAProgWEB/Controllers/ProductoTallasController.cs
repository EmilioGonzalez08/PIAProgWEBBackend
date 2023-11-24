using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIAProgWEB.Models;
using PIAProgWEB.Models.dbModels;

namespace PIAProgWEB.Controllers

{
    [Authorize (Roles = "Admin")]
    public class ProductoTallasController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public ProductoTallasController(ProyectoProWebContext context)
        {
            _context = context;
        }

        // GET: ProductoTallas
        public async Task<IActionResult> Index()
        {
            var proyectoProWebContext = await _context.ProductoTallas
                .Include(pt => pt.Producto)
                .Include(pt => pt.Talla)
                .ToListAsync();

            return View(proyectoProWebContext);
        }

        // GET: ProductoTallas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductoTallas == null)
            {
                return NotFound();
            }

            var productoTalla = await _context.ProductoTallas
                .Include(p => p.Producto)
                .Include(p => p.Talla)
                .FirstOrDefaultAsync(m => m.TallaId == id);
            if (productoTalla == null)
            {
                return NotFound();
            }

            return View(productoTalla);
        }

        // GET: ProductoTallas/Create
        public IActionResult Create()
        {
            ViewBag.Tallas = new SelectList(_context.Tallas, "TallaId", "Tamaño");
            ViewBag.Productos = new SelectList(_context.Productos, "ProductoId", "NombreProducto");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TallaId,ProductoId,Cantidad")] ProductoTallaHR productoTalla)
        {
            if (ModelState.IsValid)
            {
                ProductoTalla productoTalla1 = new ProductoTalla
                {
                    TallaId = productoTalla.TallaId,
                    ProductoId = productoTalla.ProductoId,
                    Cantidad = productoTalla.Cantidad
                };
                _context.ProductoTallas.Add(productoTalla1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Productos"] = new SelectList(_context.Productos, "ProductoId", "NombreProducto", productoTalla.ProductoId);
            ViewData["Tallas"] = new SelectList(_context.Tallas, "TallaId", "Tamaño", productoTalla.TallaId);

            return View(productoTalla);
        }

        // GET: ProductoTallas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductoTallas == null)
            {
                return NotFound();
            }

            var productoTalla = await _context.ProductoTallas.FindAsync(id);
            if (productoTalla == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", productoTalla.ProductoId);
            ViewData["TallaId"] = new SelectList(_context.Tallas, "TallaId", "TallaId", productoTalla.TallaId);
            return View(productoTalla);
        }

        // POST: ProductoTallas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TallaId,ProductoId,Cantidad")] ProductoTalla productoTalla)
        {
            if (id != productoTalla.TallaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoTalla);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoTallaExists(productoTalla.TallaId))
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
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", productoTalla.ProductoId);
            ViewData["TallaId"] = new SelectList(_context.Tallas, "TallaId", "TallaId", productoTalla.TallaId);
            return View(productoTalla);
        }

        // GET: ProductoTallas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductoTallas == null)
            {
                return NotFound();
            }

            var productoTalla = await _context.ProductoTallas
                .Include(p => p.Producto)
                .Include(p => p.Talla)
                .FirstOrDefaultAsync(m => m.TallaId == id);
            if (productoTalla == null)
            {
                return NotFound();
            }

            return View(productoTalla);
        }

        // POST: ProductoTallas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductoTallas == null)
            {
                return Problem("Entity set 'ProyectoProWebContext.ProductoTallas'  is null.");
            }
            var productoTalla = await _context.ProductoTallas.FindAsync(id);
            if (productoTalla != null)
            {
                _context.ProductoTallas.Remove(productoTalla);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoTallaExists(int id)
        {
          return (_context.ProductoTallas?.Any(e => e.TallaId == id)).GetValueOrDefault();
        }
    }
}
