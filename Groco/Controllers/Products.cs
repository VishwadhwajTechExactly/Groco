using Groco.Data;
using Groco.Models;
using Microsoft.AspNetCore.Mvc;

namespace Groco.Controllers
{
    public class Products : Controller
    {
        private readonly ApplicationDbContext _context;

        public Products(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Product> products=_context.Products.ToList();
            return View(products);
        }
    }
}
