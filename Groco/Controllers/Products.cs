using Groco.Data;
using Groco.Models;
using Groco.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categories = _context.Categories.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ProductViewModel productViewModel = new ProductViewModel
            {
                Product = new Product(),
                CategoryList=categories
            };
            return View(productViewModel);
        }

        //[HttpPost]
        //public IActionResult Create(Product product,)
        //{
        //}
    }
}
