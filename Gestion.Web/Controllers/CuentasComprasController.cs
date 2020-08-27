using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize]
    public class CuentasComprasController : Controller
    {
        private readonly ICuentasComprasRepository repository; 
        private readonly IUserHelper userHelper;

        public CuentasComprasController(ICuentasComprasRepository repository, IUserHelper userHelper)
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

            var CuentasCompras = await this.repository.GetByIdAsync(id);
            if (CuentasCompras == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(CuentasCompras);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamCuentasCompras CuentasCompras)
        {
            if (ModelState.IsValid)
            {
                CuentasCompras.Estado = true;
                await repository.CreateAsync(CuentasCompras);
                return RedirectToAction(nameof(Index));
            }
            return View(CuentasCompras);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CuentasCompras = await this.repository.GetByIdAsync(id);
            if (CuentasCompras == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(CuentasCompras);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamCuentasCompras CuentasCompras)
        {
            if (id != CuentasCompras.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(CuentasCompras);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(CuentasCompras.Id))
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
            return View(CuentasCompras);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CuentasCompras = await this.repository.GetByIdAsync(id);
            if (CuentasCompras == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            CuentasCompras.Estado = !CuentasCompras.Estado;
            await repository.DeleteAsync(CuentasCompras);
            return RedirectToAction(nameof(Index));
        }        

    }
}
