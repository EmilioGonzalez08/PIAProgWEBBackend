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
    public class ProductosController : Controller
    {
        private readonly ProyectoProWebContext _context;

        public ProductosController(ProyectoProWebContext context)
        {
            _context = context;
        }

        //GET: Productos
        public async Task<IActionResult> Index(string categoria)
        {
            IQueryable<Producto> productosQuery = _context.Productos.Include(p => p.SubCategoria);

            if (!string.IsNullOrEmpty(categoria))
            {
                categoria = categoria.ToLower(); // o .ToUpper() según tu preferencia
                productosQuery = productosQuery.Where(p => p.SubCategoria.Categoria != null && p.SubCategoria.NombreSubcategoria.ToLower() == categoria);
            }

            var productos = await productosQuery.ToListAsync();

            return View(productos);
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
            // Assuming Categoria and Subcategoria are your model classes
            ViewData["Categorias"] = new SelectList(_context.Categoria, "CategoriaId", "Nombre");
            ViewData["Subcategorias"] = new SelectList(_context.Subcategoria, "IdSubcategoria", "Nombre");

            return View();
        }

        // POST: Productos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,NombreProducto,Descripción,Precio,CategoriaId,IdSubcategoria,Imagen")] ProductoCreateHR producto)
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

            // Populate both "Categorias" and "Subcategorias" dropdowns
            ViewData["Categorias"] = new SelectList(_context.Categoria, "CategoriaId", "Nombre", producto.CategoriaId);
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId", producto.CategoriaId);
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "CategoriaId", "CategoriaId", producto.CategoriaId);
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
            if (_context.Productos == null)
            {
                return Problem("Entity set 'ProyectoProWebContext.Productos'  is null.");
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
          return (_context.Productos?.Any(e => e.ProductoId == id)).GetValueOrDefault();
        }
    }
}
