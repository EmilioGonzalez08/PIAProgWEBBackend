using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIAProgWEB.Models;
using PIAProgWEB.Models.dbModels;
using System.Security.Claims;

namespace PIAProgWEB.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public CarritoController(ProyectoProWebContext context)
        {
            _context = context;
        }

        private bool CarritoExists(int id)
        {
            return _context.Carritos.Any(e => e.UsuarioId == id);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            try
            {
                // Retrieve the current user's ID
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Ensure the user is authenticated and the user ID is not null or empty
                if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(userId))
                {
                    // Check if the product is already in the user's cart
                    var existingCartItem = _context.Carritos
                        .FirstOrDefault(c => c.UsuarioId == int.Parse(userId) && c.ProductioId == productId);

                    if (existingCartItem != null)
                    {
                        // If the product is already in the cart, you might want to update the quantity instead
                        existingCartItem.Cantidad += 1;
                    }
                    else
                    {
                        // If the product is not in the cart, create a new cart item
                        var newCartItem = new Carrito
                        {
                            UsuarioId = int.Parse(userId),
                            ProductioId = productId,
                            Cantidad = 1,
                            Fecha = DateTime.Now // or use your preferred way to set the date
                        };

                        // Add the new cart item to the context
                        _context.Carritos.Add(newCartItem);
                    }

                    // Save changes to the database
                    _context.SaveChanges();

                    // Return a success message
                    return Json(new { success = true, message = "Product added to cart" });
                }
                else
                {
                    // Return an error message if the user is not authenticated or user ID is null/empty
                    return Json(new { success = false, message = "User not authenticated" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error message
                return Json(new { success = false, message = "Error adding product to cart" });
            }
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
            if (id == null)
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
            if (id == null)
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
        public async Task<IActionResult> DeleteConfirmed(int usuarioId, int productioId)
        {
            var carrito = await _context.Carritos.FindAsync(usuarioId, productioId);

            if (carrito == null)
            {
                return NotFound();
            }

            try
            {
                _context.Carritos.Remove(carrito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return RedirectToAction(nameof(Index)); // Redirect to the index on error
            }
        }
    }
}