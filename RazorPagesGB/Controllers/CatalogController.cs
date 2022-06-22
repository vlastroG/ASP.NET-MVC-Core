using Microsoft.AspNetCore.Mvc;
using RazorPagesGB.Models;
using RazorPagesGB.Services.EmailService;

namespace RazorPagesGB.Controllers
{
    public class CatalogController : Controller
    {
        private static Catalog _catalog = new();

        private readonly IEmailService _emailService;

        private readonly string _to = "savion.paucek81@ethereal.email";

        public CatalogController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Products()
        {
            return View(_catalog);
        }

        [HttpPost]
        public IActionResult Products(Product product)
        {
            _catalog.ProductAdd(product);
            // добавить отправку email
            string body =
                "<h1>В каталог добавлен следующий товар:</h1>" +
                "<tr>" +
                    "<td>" + product.Id + " | </td>" +
                    "<td>" + product.Name + " | </td>" +
                    "<td>" + product.Description + " | </td>" +
                    "<td>" + product.Price + " | </td>" +
                    "<td><img src = \"" + @product.Image + "\" alt = \"Product photo.\"></td>" +
                "</tr>";
            _emailService.Send(new EmailDto()
            {
                Body = body,
                Subject = "Добавлен товар",
                To = _to
            });
            return RedirectToAction("Products");
        }

        [HttpPost, ActionName("ProductDelete")]
        public IActionResult ProductDeleteConfirmed(int id)
        {
            Product? product = _catalog.ProductsGetAll().FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _catalog.ProductDelete(id);
            // добавить отправку email
            string body =
                "<h1>Из каталога удален следующий товар:</h1>" +
                "<tr>" +
                    "<td>" + product.Id + " | </td>" +
                    "<td>" + product.Name + " | </td>" +
                    "<td>" + product.Description + " | </td>" +
                    "<td>" + product.Price + " | </td>" +
                    "<td><img src = \"" + @product.Image + "\" alt = \"Product photo.\"></td>" +
                "</tr>";
            _emailService.Send(new EmailDto()
            {
                Body = body,
                Subject = "Удален товар",
                To = _to
            });
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
