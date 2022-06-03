using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Models;
using Restaurante.Data;
using Microsoft.EntityFrameworkCore;

namespace Restaurante.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger,ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
        {
              return _context.Categoria != null ? 
                          View(await _context.Categoria.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Categoria'  is null.");
        }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> Menu_Desayunos()
    {
        var applicationDbContext = _context.Platillo.Include(p => p.Categoria);
            return View(await applicationDbContext.ToListAsync());
    }

    public async Task<IActionResult> Menu_Comidas()
    {
        var applicationDbContext = _context.Platillo.Include(p => p.Categoria);
            return View(await applicationDbContext.ToListAsync());
    }

    public async Task<IActionResult> Menu_Bebidas()
    {
        var applicationDbContext = _context.Platillo.Include(p => p.Categoria);
            return View(await applicationDbContext.ToListAsync());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
