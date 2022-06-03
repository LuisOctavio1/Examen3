using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Models;

namespace Restaurante.Controllers
{
    public class PlatillosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnviroment;


        public PlatillosController(ApplicationDbContext context,IWebHostEnvironment hostEnviroment)
        {
            _context = context;
            _hostEnviroment = hostEnviroment;
        }

        // GET: Platillos
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Platillo.Include(p => p.Categoria);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Platillos/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Platillo == null)
            {
                return NotFound();
            }

            var platillo = await _context.Platillo
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platillo == null)
            {
                return NotFound();
            }

            return View(platillo);
        }

        // GET: Platillos/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "Nombre");
            return View();
        }

        // POST: Platillos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Nombre,IdCategoria,Descripcion,precio,UrlImagen")] Platillo platillo)
        {
            //if (ModelState.IsValid)
            //{
                string rutaPrincipal = _hostEnviroment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if(archivos.Count > 0){
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal,@"imagenes\platillos\");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    using(var fileStream = new FileStream(Path.Combine(subidas,nombreArchivo + extension),FileMode.Create)){
                        archivos[0].CopyTo(fileStream);
                    }
                    platillo.UrlImagen = @"imagenes\platillos\" + nombreArchivo + extension;
                }
                _context.Add(platillo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "Nombre", platillo.IdCategoria);
            return View(platillo);
        }

        // GET: Platillos/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Platillo == null)
            {
                return NotFound();
            }

            var platillo = await _context.Platillo.FindAsync(id);
            if (platillo == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "Nombre", platillo.IdCategoria);
            return View(platillo);
        }

        // POST: Platillos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,IdCategoria,Descripcion,precio,UrlImagen")] Platillo platillo)
        {
            if (id != platillo.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    
                    string rutaPrincipal = _hostEnviroment.WebRootPath;
                    var archivos = HttpContext.Request.Form.Files;
                    Platillo? platilloDB= await _context.Platillo.FindAsync(id);
                        if(platilloDB!= null && platilloDB.UrlImagen != null){
                            var rutaImagen = Path.Combine(rutaPrincipal,platilloDB.UrlImagen);
                            if(System.IO.File.Exists(rutaImagen)){
                                System.IO.File.Delete(rutaImagen);
                            }
                            _context.Entry(platilloDB).State = EntityState.Detached;
                        }
                    if(archivos.Count > 0){
                        string nombreArchivo = Guid.NewGuid().ToString();
                        var subidas = Path.Combine(rutaPrincipal,@"imagenes\platillos\");
                        var extension = Path.GetExtension(archivos[0].FileName);
                        using(var fileStream = new FileStream(Path.Combine(subidas,nombreArchivo + extension),FileMode.Create)){
                            archivos[0].CopyTo(fileStream);
                        }
                        platillo.UrlImagen = @"imagenes\platillos\" + nombreArchivo + extension;
                    }
                    _context.Update(platillo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlatilloExists(platillo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "Nombre", platillo.IdCategoria);
            return View(platillo);
        }

        // GET: Platillos/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Platillo == null)
            {
                return NotFound();
            }

            var platillo = await _context.Platillo
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platillo == null)
            {
                return NotFound();
            }

            return View(platillo);
        }

        // POST: Platillos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Platillo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Platillo'  is null.");
            }
            var platillo = await _context.Platillo.FindAsync(id);
            if (platillo != null)
            {
                _context.Platillo.Remove(platillo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlatilloExists(int id)
        {
          return (_context.Platillo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
