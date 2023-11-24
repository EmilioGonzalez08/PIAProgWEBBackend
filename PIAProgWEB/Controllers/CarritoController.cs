using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIAProgWEB.Models;
using PIAProgWEB.Models.dbModels;

namespace PIAProgWEB.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public CarritoController(ProyectoProWebContext context)
        {
            _context = context;
        }

        // GET: Carrito
        public async Task<IActionResult> Index()
        {
            var proyectoProWebContext = _context.Carritos.Include(c => c.Productio).Include(c => c.Usuario);
            return View(await proyectoProWebContext.ToListAsync());
        }

        // GET: Carrito/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Carritos == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos
                .Include(c => c.Productio)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }

        // GET: Carrito/Create
        public IActionResult Create()
        {
            ViewData["ProductioId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId");
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Carrito/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,ProductioId,Cantidad,Fecha")] CarritoHR carrito)
        {
            if (ModelState.IsValid)
            {
                Carrito carrito1 = new Carrito
                {

                    UsuarioId = carrito.UsuarioId,
                    ProductioId = carrito.ProductioId,
                    Cantidad = carrito.Cantidad,
                    Fecha = carrito.Fecha

                };

                _context.Carritos.Add(carrito1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductioId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", carrito.ProductioId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", carrito.UsuarioId);
            return View(carrito);
        }

        // GET: Carrito/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carritos == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos.FindAsync(id);
            if (carrito == null)
            {
                return NotFound();
            }
            ViewData["ProductioId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", carrito.ProductioId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", carrito.UsuarioId);
            return View(carrito);
        }

        // POST: Carrito/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,ProductioId,Cantidad,Fecha")] Carrito carrito)
        {
            if (id != carrito.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarritoExists(carrito.UsuarioId))
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
            ViewData["ProductioId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", carrito.ProductioId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", carrito.UsuarioId);
            return View(carrito);
        }

        // GET: Carrito/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Carritos == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos
                .Include(c => c.Productio)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }

        // POST: Carrito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carritos == null)
            {
                return Problem("Entity set 'ProyectoProWebContext.Carritos'  is null.");
            }
            var carrito = await _context.Carritos.FindAsync(id);
            if (carrito != null)
            {
                _context.Carritos.Remove(carrito);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarritoExists(int id)
        {
          return (_context.Carritos?.Any(e => e.UsuarioId == id)).GetValueOrDefault();
        }
    }
}
