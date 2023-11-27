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
    public class SubcategoriasController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public SubcategoriasController(ProyectoProWebContext context)
        {
            _context = context;
        }


        // GET: Subcategorias
        public async Task<IActionResult> Index()
        {
            var proyectoProWebContext = _context.Subcategoria.Include(s => s.Categoria);
            return View(await proyectoProWebContext.ToListAsync());
        }

        // GET: Subcategorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Subcategoria == null)
            {
                return NotFound();
            }

            var subcategorium = await _context.Subcategoria
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(m => m.IdSubcategoria == id);
            if (subcategorium == null)
            {
                return NotFound();
            }

            return View(subcategorium);
        }

        // GET: Subcategorias/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId");
            return View();
        }

        // POST: Subcategorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSubcategoria,CategoriaId,NombreSubcategoria")] SubcategoriasHR subcategorium)
        {
            if (ModelState.IsValid)
            {
                Subcategorium subcategorium1 = new Subcategorium
                {

                    IdSubcategoria = subcategorium.IdSubcategoria,
                    CategoriaId = subcategorium.CategoriaId,
                    NombreSubcategoria = subcategorium.NombreSubcategoria

                };
                _context.Subcategoria.Add(subcategorium1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId", subcategorium.CategoriaId);
            return View(subcategorium);
        }

        // GET: Subcategorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Subcategoria == null)
            {
                return NotFound();
            }

            var subcategorium = await _context.Subcategoria.FindAsync(id);
            if (subcategorium == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId", subcategorium.CategoriaId);
            return View(subcategorium);
        }

        // POST: Subcategorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSubcategoria,CategoriaId,NombreSubcategoria")] Subcategorium subcategorium)
        {
            if (id != subcategorium.IdSubcategoria)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subcategorium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubcategoriumExists(subcategorium.IdSubcategoria))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId", subcategorium.CategoriaId);
            return View(subcategorium);
        }

        // GET: Subcategorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subcategoria == null)
            {
                return NotFound();
            }

            var subcategorium = await _context.Subcategoria
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(m => m.IdSubcategoria == id);
            if (subcategorium == null)
            {
                return NotFound();
            }

            return View(subcategorium);
        }

        // POST: Subcategorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subcategoria == null)
            {
                return Problem("Entity set 'ProyectoProWebContext.Subcategoria'  is null.");
            }
            var subcategorium = await _context.Subcategoria.FindAsync(id);
            if (subcategorium != null)
            {
                _context.Subcategoria.Remove(subcategorium);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubcategoriumExists(int id)
        {
          return (_context.Subcategoria?.Any(e => e.IdSubcategoria == id)).GetValueOrDefault();
        }
    }
}
