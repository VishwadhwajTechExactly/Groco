using System.Security.Cryptography;
using Groco.Data;
using Groco.Models;
using Groco.Utility;
using Groco.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Groco.Controllers
{
    public class Products : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _environment;

        private List<Product> products = new List<Product>();
        public Products(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Index()
        {
            List<Product> products = _context.Products.ToList();
            return View(products);
        }
        [Authorize(Roles = SD.Role_Admin)]
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
                CategoryList = categories
            };
            return View(productViewModel);
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public IActionResult Create(ProductViewModel productViewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _environment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productViewModel.Product.ImageUrl = @"\Images\product\" + fileName;
                }
                _context.Products.Add(productViewModel.Product);
                _context.SaveChanges();
                return RedirectToAction("Index", "Products");
            }
            return View(productViewModel);
        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Edit(int? id)
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
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public IActionResult Edit(ProductViewModel productViewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

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
        [Authorize(Roles = SD.Role_Admin)]
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
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product productFromDb = _context.Products.Find(id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            _context.Products.Remove(productFromDb);
            _context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
        [Authorize(Roles = SD.Role_Customer)]
        public IActionResult CustomerIndex()
        {
            List<Product> products = _context.Products.ToList();
            return View(products);
        }

        [Authorize(Roles = SD.Role_Customer)]
        public void AddToCart(int? id)
        {
            Product productFromDb = _context.Products.Find(id);
            if (productFromDb == null)
            {
                return;
            }
            products.Add(productFromDb);
        }

        [Authorize(Roles = SD.Role_Customer)]
        public IActionResult ShowCart()
        {
            Dictionary<Product,int> productDictionary = new Dictionary<Product,int>();
            for (int i = 0; i < products.Count; i++)
            {
                if (productDictionary.ContainsKey(products[i]))
                {
                    productDictionary[products[i]]++;
                }
                else
                {
                    productDictionary.Add(products[i], 1);
                }
            }
            return View(productDictionary);
        }
        [Authorize(Roles = SD.Role_Customer)]
        public IActionResult AddProduct(Product product)
        {
            products.Add(product);
            return RedirectToAction("ShowCart", "Products");
        }
        [Authorize(Roles = SD.Role_Customer)]
        public IActionResult SubtractProduct(Product product)
        {
            products.Remove(product);
            return RedirectToAction("ShowCart", "Products");

        }
        [Authorize(Roles = SD.Role_Customer)]
        public IActionResult RemoveProduct(Product product) {
            for (int i = products.Count; i >= 0; i--) { 
                if (products[i].ProductId == product.ProductId)
                {
                    products.RemoveAt(i);
                }
            }
            return RedirectToAction("ShowCart", "Products");
        }
    }
    
}
