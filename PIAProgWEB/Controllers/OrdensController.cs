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
    public class OrdensController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public OrdensController(ProyectoProWebContext context)
        {
            _context = context;
        }

        // GET: Ordens
        public async Task<IActionResult> Index()
        {
            var proyectoProWebContext = _context.Ordens.Include(o => o.EstadoOrdenNavigation).Include(o => o.Usuario);
            return View(await proyectoProWebContext.ToListAsync());
        }

        // GET: Ordens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ordens == null)
            {
                return NotFound();
            }

            var orden = await _context.Ordens
                .Include(o => o.EstadoOrdenNavigation)
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(m => m.OrdenId == id);
            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }

        // GET: Ordens/Create
        public IActionResult Create()
        {
            ViewData["EstadoOrden"] = new SelectList(_context.EstadoOrdens, "EstadoId", "EstadoId");
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Ordens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrdenId,Fecha,UsuarioId,EstadoOrden")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orden);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoOrden"] = new SelectList(_context.EstadoOrdens, "EstadoId", "EstadoId", orden.EstadoOrden);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", orden.UsuarioId);
            return View(orden);
        }

        // GET: Ordens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ordens == null)
            {
                return NotFound();
            }

            var orden = await _context.Ordens.FindAsync(id);
            if (orden == null)
            {
                return NotFound();
            }
            ViewData["EstadoOrden"] = new SelectList(_context.EstadoOrdens, "EstadoId", "EstadoId", orden.EstadoOrden);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", orden.UsuarioId);
            return View(orden);
        }

        // POST: Ordens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrdenId,Fecha,UsuarioId,EstadoOrden")] Orden orden)
        {
            if (id != orden.OrdenId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orden);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdenExists(orden.OrdenId))
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
            ViewData["EstadoOrden"] = new SelectList(_context.EstadoOrdens, "EstadoId", "EstadoId", orden.EstadoOrden);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", orden.UsuarioId);
            return View(orden);
        }

        // GET: Ordens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ordens == null)
            {
                return NotFound();
            }

            var orden = await _context.Ordens
                .Include(o => o.EstadoOrdenNavigation)
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(m => m.OrdenId == id);
            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }

        // POST: Ordens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ordens == null)
            {
                return Problem("Entity set 'ProyectoProWebContext.Ordens'  is null.");
            }
            var orden = await _context.Ordens.FindAsync(id);
            if (orden != null)
            {
                _context.Ordens.Remove(orden);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdenExists(int id)
        {
          return (_context.Ordens?.Any(e => e.OrdenId == id)).GetValueOrDefault();
        }
    }
}
