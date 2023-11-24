using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIAProgWEB.Models.dbModels;

namespace PIAProgWEB.Controllers
{
    public class CarritoesController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public CarritoesController(ProyectoProWebContext context)
        {
            _context = context;
        }

        // GET: Carritoes
        public async Task<IActionResult> Index()
        {
            // Obtén el usuario actual (ajusta esto según tu implementación de autenticación)
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            // Si el usuario no está autenticado, redirige a la página de inicio de sesión
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Busca el carrito del usuario actual
            var userCart = await _context.Carritos
                .Include(c => c.Productio)
                .Include(c => c.Usuario)
                .Where(c => c.UsuarioId == user.Id)
                .ToListAsync();

            return View(userCart);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Productos.FindAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            // Obtén el usuario actual (ajusta esto según tu implementación de autenticación)
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            // Busca el carrito del usuario actual
            var userCart = await _context.Carritos
                .Where(c => c.UsuarioId == user.Id && c.ProductioId == productId)
                .FirstOrDefaultAsync();

            // Si el usuario no tiene este producto en el carrito, crea uno
            if (userCart == null)
            {
                userCart = new Carrito
                {
                    UsuarioId = user.Id,
                    ProductioId = productId,
                    Cantidad = 1, // Ajusta esto según tus necesidades
                    Fecha = DateTime.Now,
                };

                _context.Carritos.Add(userCart);
            }
            else
            {
                // Si el producto ya está en el carrito, aumenta la cantidad
                userCart.Cantidad += 1; // Puedes ajustar esto según tus necesidades
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Carritoes/Details/5
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

        // GET: Carritoes/Create
        public IActionResult Create()
        {
            ViewData["ProductioId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId");
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Carritoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,ProductioId,Cantidad,Fecha")] Carrito carrito)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carrito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductioId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", carrito.ProductioId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", carrito.UsuarioId);
            return View(carrito);
        }

        // GET: Carritoes/Edit/5
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

        // POST: Carritoes/Edit/5
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

        // GET: Carritoes/Delete/5
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

        // POST: Carritoes/Delete/5
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
