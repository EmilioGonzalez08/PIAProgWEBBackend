using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIAProgWEB.Models;
using PIAProgWEB.Models.dbModels;

namespace PIAProgWEB.Controllers
{
    public class ImagenNovedadController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public ImagenNovedadController(ProyectoProWebContext context)
        {
            _context = context;
        }

        // GET: ImagenNovedad
        public async Task<IActionResult> Index()
        {
            var proyectoProWebContext = _context.ImagenNovedads.Include(i => i.IdNovedadNavigation);
            return View(await proyectoProWebContext.ToListAsync());
        }

        // GET: ImagenNovedad/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ImagenNovedads == null)
            {
                return NotFound();
            }

            var imagenNovedad = await _context.ImagenNovedads
                .Include(i => i.IdNovedadNavigation)
                .FirstOrDefaultAsync(m => m.IdImagen == id);
            if (imagenNovedad == null)
            {
                return NotFound();
            }

            return View(imagenNovedad);
        }

        // GET: ImagenNovedad/Create
        public IActionResult Create()
        {
            ViewData["IdNovedad"] = new SelectList(_context.Novedads, "IdNovedad", "IdNovedad");
            return View();
        }

        // POST: ImagenNovedad/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdImagen,IdNovedad,Imagen")] ImgNovedadHR imagenNovedad)
        {
            if (ModelState.IsValid)
            {
                ImagenNovedad imagenNovedad1 = new ImagenNovedad
                {
                    IdNovedad = imagenNovedad.IdNovedad,
                    IdImagen = imagenNovedad.IdImagen,
                    Imagen = imagenNovedad.Imagen
                };

                _context.ImagenNovedads.Add(imagenNovedad1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdNovedad"] = new SelectList(_context.Novedads, "IdNovedad", "IdNovedad", imagenNovedad.IdNovedad);
            return View(imagenNovedad);
        }

        // GET: ImagenNovedad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ImagenNovedads == null)
            {
                return NotFound();
            }

            var imagenNovedad = await _context.ImagenNovedads.FindAsync(id);
            if (imagenNovedad == null)
            {
                return NotFound();
            }
            ViewData["IdNovedad"] = new SelectList(_context.Novedads, "IdNovedad", "IdNovedad", imagenNovedad.IdNovedad);
            return View(imagenNovedad);
        }

        // POST: ImagenNovedad/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdImagen,IdNovedad,Imagen")] ImagenNovedad imagenNovedad)
        {
            if (id != imagenNovedad.IdImagen)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imagenNovedad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagenNovedadExists(imagenNovedad.IdImagen))
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
            ViewData["IdNovedad"] = new SelectList(_context.Novedads, "IdNovedad", "IdNovedad", imagenNovedad.IdNovedad);
            return View(imagenNovedad);
        }

        // GET: ImagenNovedad/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ImagenNovedads == null)
            {
                return NotFound();
            }

            var imagenNovedad = await _context.ImagenNovedads
                .Include(i => i.IdNovedadNavigation)
                .FirstOrDefaultAsync(m => m.IdImagen == id);
            if (imagenNovedad == null)
            {
                return NotFound();
            }

            return View(imagenNovedad);
        }

        // POST: ImagenNovedad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ImagenNovedads == null)
            {
                return Problem("Entity set 'ProyectoProWebContext.ImagenNovedads'  is null.");
            }
            var imagenNovedad = await _context.ImagenNovedads.FindAsync(id);
            if (imagenNovedad != null)
            {
                _context.ImagenNovedads.Remove(imagenNovedad);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImagenNovedadExists(int id)
        {
            return (_context.ImagenNovedads?.Any(e => e.IdImagen == id)).GetValueOrDefault();
        }
    }
}
