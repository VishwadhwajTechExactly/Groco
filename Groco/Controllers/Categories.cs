using Groco.Data;
using Groco.Models;
using Groco.Utility;
using Groco.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Groco.Controllers
{
    public class Categories : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _environment;

        public Categories(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Index()
        {
            List<Category> categories= _context.Categories.ToList();
            return View(categories);
        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public IActionResult Create(Category category,IFormFile? file) {
            if (ModelState.IsValid) {
                string wwwRootPath = _environment.WebRootPath;
                if (file != null) { 
                    string fileName=Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                    string categoryPath=Path.Combine(wwwRootPath, @"Images\Category");
                    using (var fileStream = new FileStream(Path.Combine(categoryPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    category.ImageUrl = @"\Images\Category\" + fileName;
                }
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index","Categories");
            }
            return View();
        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Edit(int? id) {
            if (id == null)
            {
                return NotFound();
            }
            Category categoryFromDb = _context.Categories.Find(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public IActionResult Edit(Category category,IFormFile? file)
        {
            
            if (ModelState.IsValid) {
                Category categoryFromDb = _context.Categories.Find(category.Id);
                if (file != null)
                {
                    string wwwRootPath = _environment.WebRootPath;

                    if (string.IsNullOrEmpty(categoryFromDb.ImageUrl) == false)
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, categoryFromDb.ImageUrl);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string categoryPath = Path.Combine(wwwRootPath, @"Images\Category");
                    using (var fileStream = new FileStream(Path.Combine(categoryPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    category.ImageUrl = @"\Images\Category\" + fileName;
                }
                else
                {
                    category.ImageUrl = categoryFromDb.ImageUrl;
                }
                _context.ChangeTracker.Clear();
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index", "Categories");

            }
            return View(category);
        }
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Delete(int? id) { 
            if(id == null)
            {
                return NotFound();
            }
            Category categoryFromDb = _context.Categories.Find(id);
            if (categoryFromDb == null) { 
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id) {
            if (id == null) {
                return NotFound();
            }
            Category categoryFromDb= _context.Categories.Find(id);
            if (categoryFromDb == null) {
                return NotFound();
            }
            _context.Categories.Remove(categoryFromDb);
            _context.SaveChanges();
            return RedirectToAction("Index", "Categories");
        }

        [Authorize(Roles = SD.Role_Customer)]
        public IActionResult CustomerIndex() {
            List<Category> categoryList=_context.Categories.Include(u=>u.Products).ToList();
            return View(categoryList);
        }
    }
}
