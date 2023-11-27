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
    public class EstadoOrdenController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public EstadoOrdenController(ProyectoProWebContext context)
        {
            _context = context;
        }

        // GET: EstadoOrden
        public async Task<IActionResult> Index()
        {
              return _context.EstadoOrdens != null ? 
                          View(await _context.EstadoOrdens.ToListAsync()) :
                          Problem("Entity set 'ProyectoProWebContext.EstadoOrdens'  is null.");
        }

        // GET: EstadoOrden/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EstadoOrdens == null)
            {
                return NotFound();
            }

            var estadoOrden = await _context.EstadoOrdens
                .FirstOrDefaultAsync(m => m.EstadoId == id);
            if (estadoOrden == null)
            {
                return NotFound();
            }

            return View(estadoOrden);
        }

        // GET: EstadoOrden/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadoOrden/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EstadoId,Estado")] EstadoOrden estadoOrden)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoOrden);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadoOrden);
        }

        // GET: EstadoOrden/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EstadoOrdens == null)
            {
                return NotFound();
            }

            var estadoOrden = await _context.EstadoOrdens.FindAsync(id);
            if (estadoOrden == null)
            {
                return NotFound();
            }
            return View(estadoOrden);
        }

        // POST: EstadoOrden/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EstadoId,Estado")] EstadoOrden estadoOrden)
        {
            if (id != estadoOrden.EstadoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoOrden);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoOrdenExists(estadoOrden.EstadoId))
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
            return View(estadoOrden);
        }

        // GET: EstadoOrden/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EstadoOrdens == null)
            {
                return NotFound();
            }

            var estadoOrden = await _context.EstadoOrdens
                .FirstOrDefaultAsync(m => m.EstadoId == id);
            if (estadoOrden == null)
            {
                return NotFound();
            }

            return View(estadoOrden);
        }

        // POST: EstadoOrden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EstadoOrdens == null)
            {
                return Problem("Entity set 'ProyectoProWebContext.EstadoOrdens'  is null.");
            }
            var estadoOrden = await _context.EstadoOrdens.FindAsync(id);
            if (estadoOrden != null)
            {
                _context.EstadoOrdens.Remove(estadoOrden);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoOrdenExists(int id)
        {
          return (_context.EstadoOrdens?.Any(e => e.EstadoId == id)).GetValueOrDefault();
        }
    }
}
