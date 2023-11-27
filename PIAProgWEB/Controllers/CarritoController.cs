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

        [HttpPost]
        public IActionResult RealizarCompra()
        {
            try
            {
                // Obtén el ID del usuario actual
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Asegúrate de que el usuario esté autenticado y el ID de usuario no sea nulo o vacío
                if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(userId))
                {
                    // Obtén la lista de productos en el carrito
                    var productosEnCarrito = _context.Carritos.ToList();

                    // Crea una nueva orden
                    var nuevaOrden = new Orden
                    {
                        Fecha = DateTime.Now,
                        UsuarioId = int.Parse(userId),
                        // Otras propiedades de la orden que puedas necesitar
                    };
                    nuevaOrden.EstadoOrden = 1;

                    // Agrega la orden a la base de datos
                    _context.Ordens.Add(nuevaOrden);
                    _context.SaveChanges();

                    // Itera sobre los productos en el carrito y crea detalles de orden
                    foreach (var productoEnCarrito in productosEnCarrito)
                    {
                        // Obtén la información del producto
                        var productInfo = _context.Productos
                            .Where(p => p.ProductoId == productoEnCarrito.ProductioId)
                            .FirstOrDefault();

                        // Obtén la información del producto-talla
                        var productoTalla = _context.ProductoTallas
                            .Include(pt => pt.Talla) // Cargar la entidad relacionada Talla
                            .Where(pt => pt.ProductoId == productoEnCarrito.ProductioId)
                            .FirstOrDefault();

                        // Asegúrate de que haya suficientes productos disponibles
                        if (productoTalla != null && productoTalla.Cantidad >= productoEnCarrito.Cantidad)
                        {
                            // Actualiza la cantidad disponible en el producto-talla
                            productoTalla.Cantidad -= productoEnCarrito.Cantidad;
                        }
                        else
                        {
                            // Retorna un mensaje de error si no hay suficientes productos disponibles
                            return Json(new { success = false, message = "No hay suficientes productos disponibles" });
                        }

                        // Crea un detalle de orden
                        var detalleOrden = new DetalleOrden
                        {
                            OrdenId = nuevaOrden.OrdenId,
                            ProductoId = productoEnCarrito.ProductioId,
                            Cantidad = productoEnCarrito.Cantidad,
                            PrecioUnitario = productInfo.Precio
                            // Otras propiedades del detalle de orden que puedas necesitar
                        };

                        // Agrega el detalle de orden a la base de datos
                        _context.DetalleOrdens.Add(detalleOrden);
                    }

                    // Elimina los productos del carrito después de la compra
                    _context.Carritos.RemoveRange(productosEnCarrito);

                    // Guarda los cambios en la base de datos después de actualizar la cantidad disponible
                    _context.SaveChanges();

                    // Redirige a la página de inicio u otra página según tus necesidades
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Retorna un mensaje de error si el usuario no está autenticado o el ID de usuario es nulo/vacío
                    return Json(new { success = false, message = "Usuario no autenticado" });
                }
            }
            catch (Exception ex)
            {
                // Registra la excepción
                // Retorna un mensaje de error
                return Json(new { success = false, message = "Error al realizar la compra" });
            }
        }


        private bool CarritoExists(int id)
        {
            return _context.Carritos.Any(e => e.UsuarioId == id);
        }
        private object GetProductInfo(int productId)
        {
            var productInfo = _context.Productos
                .Where(p => p.ProductoId == productId)
                .Select(p => new
                {
                    Id = p.ProductoId,
                    Name = p.NombreProducto,
                    Description = p.Descripción,
                    Price = p.Precio,
                    // Agrega otras propiedades relevantes del producto
                })
                .FirstOrDefault();

            return productInfo;
        }

        


        private object GetUpdatedCartInfo(int userId)
        {
            var cartInfo = _context.Carritos
                .Where(c => c.UsuarioId == userId)
                .Select(c => new
                {
                    ProductId = c.ProductioId,
                    Quantity = c.Cantidad,
                    Date = c.Fecha,
                    UserId = c.UsuarioId,
                    Price = _context.Productos
                                .Where(p => p.ProductoId == c.ProductioId)
                                .Select(p => p.Precio)
                                .FirstOrDefault(),
                    // Agrega otras propiedades relevantes del carrito
                })
                .ToList();

            return cartInfo;
        }
        [HttpPost]
        public IActionResult AddToCart(int productId, string selectedSize, int selectedQuantity)
        {
            try
            {
                // Retrieve the current user's ID
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Ensure the user is authenticated and the user ID is not null or empty
                if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(userId))
                {
                    // Find the product size to check availability
                    var availableQuantity = _context.ProductoTallas
                        .Where(pt => pt.ProductoId == productId && pt.Talla.Tamaño == selectedSize)
                        .Select(pt => pt.Cantidad)
                        .FirstOrDefault();

                    // Check if the product is already in the user's cart
                    var existingCartItem = _context.Carritos
                        .FirstOrDefault(c => c.UsuarioId == int.Parse(userId) && c.ProductioId == productId);

                    if (existingCartItem != null)
                    {
                        // If the product is already in the cart, you might want to update the quantity instead
                        if (existingCartItem.Cantidad + selectedQuantity <= availableQuantity)
                        {
                            existingCartItem.Cantidad += selectedQuantity;

                            // Save changes to the database
                            _context.SaveChanges();

                            // Get updated cart information
                            var updatedCart = GetUpdatedCartInfo(int.Parse(userId));

                            // Return product and cart information
                            return Json(new { success = true, message = "Product added to cart", product = GetProductInfo(productId), cart = updatedCart });
                        }
                    }
                    else
                    {
                        // If the product is not in the cart, create a new cart item
                        if (selectedQuantity <= availableQuantity)
                        {
                            var newCartItem = new Carrito
                            {
                                UsuarioId = int.Parse(userId),
                                ProductioId = productId,
                                Cantidad = selectedQuantity,
                                Fecha = DateTime.Now // or use your preferred way to set the date
                            };

                            // Add the new cart item to the context
                            _context.Carritos.Add(newCartItem);

                            // Save changes to the database
                            _context.SaveChanges();

                            // Get updated cart information
                            var updatedCart = GetUpdatedCartInfo(int.Parse(userId));

                            // Return product and cart information
                            return Json(new { success = true, message = "Product added to cart", product = GetProductInfo(productId), cart = updatedCart });
                        }
                    }

                    // If the code reaches here, it means the quantity exceeded the available quantity
                    return Json(new { success = false, message = "Exceeded available quantity" });
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
            // Retrieve the current user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(userId))
            {
                // Filter carritos for the current user
                var proyectoProWebContext = _context.Carritos
                    .Where(c => c.UsuarioId == int.Parse(userId))
                    .Include(c => c.Productio)
                    .Include(c => c.Usuario);

                return View(await proyectoProWebContext.ToListAsync());
            }
            else
            {
                // Handle the case when the user is not authenticated
                return RedirectToAction("Login", "Account");
            }
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