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
    public class TallaController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public TallaController(ProyectoProWebContext context)
        {
            _context = context;
        }

        // GET: Talla
        public async Task<IActionResult> Index()
        {
              return _context.Tallas != null ? 
                          View(await _context.Tallas.ToListAsync()) :
                          Problem("Entity set 'ProyectoProWebContext.Tallas'  is null.");
        }

        // GET: Talla/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tallas == null)
            {
                return NotFound();
            }

            var talla = await _context.Tallas
                .FirstOrDefaultAsync(m => m.TallaId == id);
            if (talla == null)
            {
                return NotFound();
            }

            return View(talla);
        }

        // GET: Talla/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Talla/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TallaId,Tamaño")] Talla talla)
        {
            if (ModelState.IsValid)
            {
                _context.Add(talla);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(talla);
        }

        // GET: Talla/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tallas == null)
            {
                return NotFound();
            }

            var talla = await _context.Tallas.FindAsync(id);
            if (talla == null)
            {
                return NotFound();
            }
            return View(talla);
        }

        // POST: Talla/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TallaId,Tamaño")] Talla talla)
        {
            if (id != talla.TallaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(talla);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TallaExists(talla.TallaId))
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
            return View(talla);
        }

        // GET: Talla/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tallas == null)
            {
                return NotFound();
            }

            var talla = await _context.Tallas
                .FirstOrDefaultAsync(m => m.TallaId == id);
            if (talla == null)
            {
                return NotFound();
            }

            return View(talla);
        }

        // POST: Talla/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tallas == null)
            {
                return Problem("Entity set 'ProyectoProWebContext.Tallas'  is null.");
            }
            var talla = await _context.Tallas.FindAsync(id);
            if (talla != null)
            {
                _context.Tallas.Remove(talla);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TallaExists(int id)
        {
          return (_context.Tallas?.Any(e => e.TallaId == id)).GetValueOrDefault();
        }
    }
}
