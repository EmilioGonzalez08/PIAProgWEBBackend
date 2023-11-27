using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIAProgWEB.Models.dbModels;

namespace PIAProgWEB.Controllers
{
    public class DetalleOrdenController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public DetalleOrdenController(ProyectoProWebContext context)
        {
            _context = context;
        }

        // GET: DetalleOrden
        public async Task<IActionResult> Index()
        {
            var proyectoProWebContext = _context.DetalleOrdens.Include(d => d.Orden).Include(d => d.Producto);
            return View(await proyectoProWebContext.ToListAsync());
        }

        // GET: DetalleOrden/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DetalleOrdens == null)
            {
                return NotFound();
            }

            var detalleOrden = await _context.DetalleOrdens
                .Include(d => d.Orden)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.OrdenId == id);
            if (detalleOrden == null)
            {
                return NotFound();
            }

            return View(detalleOrden);
        }

        // GET: DetalleOrden/Create
        public IActionResult Create()
        {
            ViewData["OrdenId"] = new SelectList(_context.Ordens, "OrdenId", "OrdenId");
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId");
            return View();
        }

        // POST: DetalleOrden/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrdenId,ProductoId,Cantidad,PrecioUnitario")] DetalleOrden detalleOrden)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleOrden);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrdenId"] = new SelectList(_context.Ordens, "OrdenId", "OrdenId", detalleOrden.OrdenId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", detalleOrden.ProductoId);
            return View(detalleOrden);
        }

        // GET: DetalleOrden/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DetalleOrdens == null)
            {
                return NotFound();
            }

            var detalleOrden = await _context.DetalleOrdens.FindAsync(id);
            if (detalleOrden == null)
            {
                return NotFound();
            }
            ViewData["OrdenId"] = new SelectList(_context.Ordens, "OrdenId", "OrdenId", detalleOrden.OrdenId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", detalleOrden.ProductoId);
            return View(detalleOrden);
        }

        // POST: DetalleOrden/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrdenId,ProductoId,Cantidad,PrecioUnitario")] DetalleOrden detalleOrden)
        {
            if (id != detalleOrden.OrdenId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleOrden);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleOrdenExists(detalleOrden.OrdenId))
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
            ViewData["OrdenId"] = new SelectList(_context.Ordens, "OrdenId", "OrdenId", detalleOrden.OrdenId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", detalleOrden.ProductoId);
            return View(detalleOrden);
        }

        // GET: DetalleOrden/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DetalleOrdens == null)
            {
                return NotFound();
            }

            var detalleOrden = await _context.DetalleOrdens
                .Include(d => d.Orden)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.OrdenId == id);
            if (detalleOrden == null)
            {
                return NotFound();
            }

            return View(detalleOrden);
        }

        // POST: DetalleOrden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DetalleOrdens == null)
            {
                return Problem("Entity set 'ProyectoProWebContext.DetalleOrdens'  is null.");
            }
            var detalleOrden = await _context.DetalleOrdens.FindAsync(id);
            if (detalleOrden != null)
            {
                _context.DetalleOrdens.Remove(detalleOrden);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleOrdenExists(int id)
        {
          return (_context.DetalleOrdens?.Any(e => e.OrdenId == id)).GetValueOrDefault();
        }
    }
}
