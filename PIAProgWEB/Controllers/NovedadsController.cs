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
    public class NovedadsController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public NovedadsController(ProyectoProWebContext context)
        {
            _context = context;
        }

        // GET: Novedads
        public async Task<IActionResult> Index()
        {
            var proyectoProWebContext = _context.Novedads.Include(n => n.IdEstacionNavigation);
            return View(await proyectoProWebContext.ToListAsync());
        }

        // GET: Novedads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Novedads == null)
            {
                return NotFound();
            }

            var novedad = await _context.Novedads
                .Include(n => n.IdEstacionNavigation)
                .FirstOrDefaultAsync(m => m.IdNovedad == id);
            if (novedad == null)
            {
                return NotFound();
            }

            return View(novedad);
        }
        [Authorize (Roles = "Admin")]
        // GET: Novedads/Create
        public IActionResult Create()
        {
            ViewData["IdEstacion"] = new SelectList(_context.Estaciones, "IdEstacion", "IdEstacion");
            return View();
        }

        // POST: Novedads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNovedad,Descripcion,IdEstacion")] NovedasHR novedad)
        {
            if (ModelState.IsValid)
            {
                Novedad novedad1 = new Novedad
                {
                    IdNovedad = novedad.IdNovedad,
                    Descripcion = novedad.Descripcion,
                    IdEstacion = novedad.IdEstacion,

                };
                _context.Novedads.Add(novedad1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstacion"] = new SelectList(_context.Estaciones, "IdEstacion", "IdEstacion", novedad.IdEstacion);
            return View(novedad);
        }
        [Authorize(Roles = "Admin")]
        // GET: Novedads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Novedads == null)
            {
                return NotFound();
            }

            var novedad = await _context.Novedads.FindAsync(id);
            if (novedad == null)
            {
                return NotFound();
            }
            ViewData["IdEstacion"] = new SelectList(_context.Estaciones, "IdEstacion", "IdEstacion", novedad.IdEstacion);
            return View(novedad);
        }
        [Authorize(Roles = "Admin")]
        // POST: Novedads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNovedad,Descripcion,IdEstacion")] Novedad novedad)
        {
            if (id != novedad.IdNovedad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(novedad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NovedadExists(novedad.IdNovedad))
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
            ViewData["IdEstacion"] = new SelectList(_context.Estaciones, "IdEstacion", "IdEstacion", novedad.IdEstacion);
            return View(novedad);
        }
        [Authorize(Roles = "Admin")]
        // GET: Novedads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Novedads == null)
            {
                return NotFound();
            }

            var novedad = await _context.Novedads
                .Include(n => n.IdEstacionNavigation)
                .FirstOrDefaultAsync(m => m.IdNovedad == id);
            if (novedad == null)
            {
                return NotFound();
            }

            return View(novedad);
        }
        [Authorize(Roles = "Admin")]
        // POST: Novedads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Novedads == null)
            {
                return Problem("Entity set 'ProyectoProWebContext.Novedads'  is null.");
            }
            var novedad = await _context.Novedads.FindAsync(id);
            if (novedad != null)
            {
                _context.Novedads.Remove(novedad);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NovedadExists(int id)
        {
          return (_context.Novedads?.Any(e => e.IdNovedad == id)).GetValueOrDefault();
        }
    }
}
