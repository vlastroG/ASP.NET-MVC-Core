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
            _catalog.Products.Add(product);
            return View(_catalog);
        }


        [HttpGet]
        public IActionResult ProductsAdd()
        {
            return View();
        }
    }
}
