using Groco.Data;
using Groco.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            List<Category> categories= _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

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
                    category.ImageUrl = @"\Images\Category" + fileName;
                }
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

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

        [HttpPost]
        public IActionResult Edit(Category category,IFormFile? file)
        {
            if (ModelState.IsValid) { 
                string wwwRootPath= _environment.WebRootPath;
                Category categoryFromDb=_context.Categories.Find(category.Id);
                if (string.IsNullOrEmpty(categoryFromDb.ImageUrl)==false)
                {
                    string oldImagePath= Path.Combine(wwwRootPath,categoryFromDb.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                if (file != null) {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string categoryPath = Path.Combine(wwwRootPath, @"Images\Category");
                    using (var fileStream = new FileStream(Path.Combine(categoryPath, fileName), FileMode.Create)) { 
                        file.CopyTo(fileStream);
                    }
                    category.ImageUrl=@"\Images\Category\"+fileName;
                }
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

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
            return RedirectToAction("Index");
        }
    }
}
