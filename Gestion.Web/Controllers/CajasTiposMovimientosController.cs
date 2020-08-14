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
    public class CajasTiposMovimientosController : Controller
    {
        private readonly ICajasTiposMovimientosRepository repository; 
        private readonly IUserHelper userHelper;

        public CajasTiposMovimientosController(ICajasTiposMovimientosRepository repository, IUserHelper userHelper)
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

            var CajasTiposMovimientos = await this.repository.GetByIdAsync(id);
            if (CajasTiposMovimientos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(CajasTiposMovimientos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamCajasMovimientosTipos CajasTiposMovimientos)
        {
            if (ModelState.IsValid)
            {
                CajasTiposMovimientos.Estado = true;
                await repository.CreateAsync(CajasTiposMovimientos);
                return RedirectToAction(nameof(Index));
            }
            return View(CajasTiposMovimientos);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CajasTiposMovimientos = await this.repository.GetByIdAsync(id);
            if (CajasTiposMovimientos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(CajasTiposMovimientos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamCajasMovimientosTipos CajasTiposMovimientos)
        {
            if (id != CajasTiposMovimientos.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CajasTiposMovimientos.Estado = true;
                    await repository.UpdateAsync(CajasTiposMovimientos);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(CajasTiposMovimientos.Id))
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
            return View(CajasTiposMovimientos);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CajasTiposMovimientos = await this.repository.GetByIdAsync(id);
            if (CajasTiposMovimientos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(CajasTiposMovimientos);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var CajasTiposMovimientos = await repository.GetByIdAsync(id);
            await repository.DeleteAsync(CajasTiposMovimientos);
            return RedirectToAction(nameof(Index));
        }

    }
}
