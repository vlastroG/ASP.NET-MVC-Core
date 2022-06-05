using Microsoft.AspNetCore.Mvc;
using RazorPagesGB.Models;

namespace RazorPagesGB.Controllers
{
    public class CatalogController : Controller
    {
        private static Catalog _catalog = new();

        [HttpGet]
        public IActionResult Products()
        {
            return View(_catalog);
        }

        [HttpPost]
        public IActionResult Products(Product product)
        {
            _catalog.ProductAdd(product);
            return RedirectToAction("Products");
        }

        [HttpPost, ActionName("ProductDelete")]
        public IActionResult ProductDeleteConfirmed(int id)
        {
            if (_catalog.ProductsGetAll()
                .FirstOrDefault(p => p.Id == id) == null)
            {
                return NotFound();
            }
            _catalog.ProductDelete(id);
            return RedirectToAction("Products");
        }


        [HttpGet]
        public IActionResult ProductsAdd()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ProductDelete(int id)
        {
            var product = _catalog.ProductsGetAll()
                .FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
