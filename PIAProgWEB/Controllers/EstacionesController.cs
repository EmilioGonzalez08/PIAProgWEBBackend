using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIAProgWEB.Models.dbModels;

namespace PIAProgWEB.Controllers
{
    [Authorize (Roles = "Admin")]
    public class EstacionesController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public EstacionesController(ProyectoProWebContext context)
        {
            _context = context;
        }

        // GET: Estaciones
        public async Task<IActionResult> Index()
        {
              return _context.Estaciones != null ? 
                          View(await _context.Estaciones.ToListAsync()) :
                          Problem("Entity set 'ProyectoProWebContext.Estaciones'  is null.");
        }

        // GET: Estaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Estaciones == null)
            {
                return NotFound();
            }

            var estacione = await _context.Estaciones
                .FirstOrDefaultAsync(m => m.IdEstacion == id);
            if (estacione == null)
            {
                return NotFound();
            }

            return View(estacione);
        }

        // GET: Estaciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Estaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEstacion,Estacion")] Estacione estacione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estacione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estacione);
        }

        // GET: Estaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Estaciones == null)
            {
                return NotFound();
            }

            var estacione = await _context.Estaciones.FindAsync(id);
            if (estacione == null)
            {
                return NotFound();
            }
            return View(estacione);
        }

        // POST: Estaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEstacion,Estacion")] Estacione estacione)
        {
            if (id != estacione.IdEstacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estacione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstacioneExists(estacione.IdEstacion))
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
            return View(estacione);
        }

        // GET: Estaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Estaciones == null)
            {
                return NotFound();
            }

            var estacione = await _context.Estaciones
                .FirstOrDefaultAsync(m => m.IdEstacion == id);
            if (estacione == null)
            {
                return NotFound();
            }

            return View(estacione);
        }

        // POST: Estaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Estaciones == null)
            {
                return Problem("Entity set 'ProyectoProWebContext.Estaciones'  is null.");
            }
            var estacione = await _context.Estaciones.FindAsync(id);
            if (estacione != null)
            {
                _context.Estaciones.Remove(estacione);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstacioneExists(int id)
        {
          return (_context.Estaciones?.Any(e => e.IdEstacion == id)).GetValueOrDefault();
        }
    }
}
