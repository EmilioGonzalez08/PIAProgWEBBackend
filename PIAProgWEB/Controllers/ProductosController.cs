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
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public ProductosController(ProyectoProWebContext context)
        {
            _context = context;
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoId == id);
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            var proyectoProWebContext = _context.Productos
                .Include(p => p.SubCategoria)
                .Include(p => p.ProductoTallas)
                    .ThenInclude(pt => pt.Talla);

            return View(await proyectoProWebContext.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.SubCategoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }



        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Subcategoria, "IdSubcategoria", "NombreSubcategoria");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,NombreProducto,Descripción,Precio,CategoriaId,Imagen")] ProductosHR producto)
        {
            if (ModelState.IsValid)
            {
                Producto producto1 = new Producto
                {

                    ProductoId = producto.ProductoId,
                    NombreProducto = producto.NombreProducto,
                    Descripción = producto.Descripción,
                    Precio = producto.Precio,
                    CategoriaId = producto.CategoriaId,
                    Imagen = producto.Imagen

                };

                _context.Productos.Add(producto1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Subcategoria, "IdSubcategoria", "IdSubcategoria", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Subcategoria, "IdSubcategoria", "IdSubcategoria", producto.CategoriaId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,NombreProducto,Descripción,Precio,CategoriaId,Imagen")] Producto producto)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.ProductoId))
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
            ViewData["CategoriaId"] = new SelectList(_context.Subcategoria, "IdSubcategoria", "IdSubcategoria", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.SubCategoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            // Eliminar asignaciones de tallas
            var productoTallas = _context.ProductoTallas.Where(pt => pt.ProductoId == id);
            _context.ProductoTallas.RemoveRange(productoTallas);

            // Eliminar registros relacionados en Carrito
            var carritoItems = _context.Carritos.Where(ci => ci.ProductioId == id);
            _context.Carritos.RemoveRange(carritoItems);

            // Eliminar el producto
            _context.Productos.Remove(producto);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
