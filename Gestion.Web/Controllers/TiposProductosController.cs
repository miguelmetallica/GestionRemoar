using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,TiposProductos")]
    public class TiposProductosController : Controller
    {
        private readonly ITiposProductosRepository repository; 
        private readonly IUserHelper userHelper;

        public TiposProductosController(ITiposProductosRepository repository, IUserHelper userHelper)
        {
            this.repository = repository;
            this.userHelper = userHelper;
        }

        public IActionResult Index()
        {
            return View(repository.GetAll());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var TiposProductos = await this.repository.GetByIdAsync(id);
            if (TiposProductos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(TiposProductos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamTiposProductos TiposProductos)
        {
            if (ModelState.IsValid)
            {
                TiposProductos.Estado = true;
                await repository.CreateAsync(TiposProductos);
                return RedirectToAction(nameof(Index));
            }
            return View(TiposProductos);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var TiposProductos = await this.repository.GetByIdAsync(id);
            if (TiposProductos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(TiposProductos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamTiposProductos TiposProductos)
        {
            if (id != TiposProductos.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(TiposProductos);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(TiposProductos.Id))
                    {
                        return new NotFoundViewResult("NoExiste");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(TiposProductos);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var TiposProductos = await this.repository.GetByIdAsync(id);
            if (TiposProductos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            TiposProductos.Estado = !TiposProductos.Estado;
            await repository.DeleteAsync(TiposProductos);
            return RedirectToAction(nameof(Index));
        }        

    }
}
