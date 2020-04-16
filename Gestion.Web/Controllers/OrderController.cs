using Gestion.Web.Data;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository repository;
        private readonly IProductosRepository productosRepository;

        public OrderController(IOrderRepository repository,IProductosRepository productosRepository)
        {
            this.repository = repository;
            this.productosRepository = productosRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await repository.GetOrdersAsync(this.User.Identity.Name);
            return View(model);            
        }

        public async Task<IActionResult> Create()
        {
            var model = await this.repository.GetDetailTempsAsync(this.User.Identity.Name);
            return this.View(model);
        }

        public IActionResult AddProduct()
        {
            var model = new AddItemViewModel
            {
                Cantidad = 1,
                Productos = this.productosRepository.GetComboProducts()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddItemViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await this.repository.AddItemToOrderAsync(model, this.User.Identity.Name);
                return this.RedirectToAction("Create");
            }

            return this.View(model);
        }

    }
}
