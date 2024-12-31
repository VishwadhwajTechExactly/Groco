using System.Security.Cryptography;
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

        private readonly IWebHostEnvironment _environment;
        public Products(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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

        [HttpPost]
        public IActionResult Create(ProductViewModel productViewModel, IFormFile? file)
        {
            if (ModelState.IsValid) {
                string wwwRootPath = _environment.WebRootPath;
                if (file != null) { 
                    string fileName=Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                    string productPath=Path.Combine(wwwRootPath, @"Images\Product");

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productViewModel.Product.ImageUrl = @"\Images\product\" + fileName;
                }
                _context.Products.Add(productViewModel.Product);
                _context.SaveChanges();
                return RedirectToAction("Index","Products");
            }
            return View(productViewModel);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productFromDb = _context.Products.Find(id);
            IEnumerable<SelectListItem> categoryList = _context.Categories
                .Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            if (productFromDb == null)
            {
                return NotFound();
            }
            ProductViewModel productViewModel = new ProductViewModel
            {
                Product = productFromDb,
                CategoryList = categoryList
            };
            return View(productViewModel);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel productViewModel,IFormFile? file)
        {
            if (ModelState.IsValid) {

                Product productFromdb = _context.Products.Find(productViewModel.Product.ProductId);
                if (file != null)
                {
                    string wwwRootPath = _environment.WebRootPath;
                    if (string.IsNullOrEmpty(productFromdb.ImageUrl) == false)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productFromdb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productViewModel.Product.ImageUrl = @"\Images\Product\" + fileName;
                }
                else
                {
                    productViewModel.Product.ImageUrl = productFromdb.ImageUrl;
                }
                _context.ChangeTracker.Clear();
                _context.Products.Update(productViewModel.Product);
                _context.SaveChanges();
                return RedirectToAction("Index", "Products");

            }
            return View(productViewModel);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productFromDb = _context.Products.Find(id);
            IEnumerable<SelectListItem> categoryList = _context.Categories
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            if (productFromDb == null)
            {
                return NotFound();
            }
            ProductViewModel productViewModel = new ProductViewModel
            {
                Product = productFromDb,
                CategoryList = categoryList
            };
            return View(productViewModel);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product productFromDb= _context.Products.Find(id);
            if (productFromDb == null) { 
                return NotFound();
            }
            _context.Products.Remove(productFromDb);
            _context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
    }
}
