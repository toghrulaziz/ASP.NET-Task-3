using ASP.NET_Task3.Data;
using ASP.NET_Task3.Models;
using ASP.NET_Task3.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Task3.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ProductController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // GetAll +
        public IActionResult Index()
        {
            var products = _appDbContext.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        // Get +
        public async Task<IActionResult> Get(int id)
        {
            var product = await _appDbContext.Products.Include(p => p.Category).Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Add +
        public IActionResult Add()
        {
            var categories = _appDbContext.Categories.ToList();
            var tags = _appDbContext.Tags.ToList();
            ViewData["Categories"] = categories;
            ViewData["Tags"] = tags;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                var tags = _appDbContext.Tags.Where(t => product.TagIds.Contains(t.Id)).ToList();

                var newProduct = new Product
                {
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Title = product.Title,
                    Tags = tags
                };
                _appDbContext.Products.Add(newProduct);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }


        // Delete +
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is not null)
            {
                _appDbContext.Products.Remove(product);
                await _appDbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }


        // Update +
        public async Task<IActionResult> Update(int id)
        {
            var product = await _appDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

            var categories = _appDbContext.Categories.ToList();
            var tags = _appDbContext.Tags.ToList();

            ViewData["Categories"] = categories;
            ViewData["Tags"] = tags;

            var editViewModel = new EditProductViewModel
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Title = product.Title,
            };
            return View(editViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Update(EditProductViewModel updatedProduct)
        {
            if (ModelState.IsValid)
            {
                var productToUpdate = await _appDbContext.Products
                    .Include(p => p.Tags)
                    .FirstOrDefaultAsync(p => p.Id == updatedProduct.Id);

                if (productToUpdate == null)
                {
                    return NotFound();
                }

                productToUpdate.CategoryId = updatedProduct.CategoryId;
                productToUpdate.Price = updatedProduct.Price;
                productToUpdate.Description = updatedProduct.Description;
                productToUpdate.ImageUrl = updatedProduct.ImageUrl;
                productToUpdate.Title = updatedProduct.Title;
                

                await _appDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else {
                return View(updatedProduct);
            }
            
        }

    }
}
