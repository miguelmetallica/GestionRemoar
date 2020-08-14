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
    public class CajasMovimientosController : Controller
    {
        private readonly ICajasMovimientosRepository repository; 
        private readonly IUserHelper userHelper;

        public CajasMovimientosController(ICajasMovimientosRepository repository, IUserHelper userHelper)
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

            var CajasMovimientos = await this.repository.GetByIdAsync(id);
            if (CajasMovimientos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(CajasMovimientos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CajasMovimientos CajasMovimientos)
        {
            if (ModelState.IsValid)
            {                
                await repository.CreateAsync(CajasMovimientos);
                return RedirectToAction(nameof(Index));
            }
            return View(CajasMovimientos);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CajasMovimientos = await this.repository.GetByIdAsync(id);
            if (CajasMovimientos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(CajasMovimientos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CajasMovimientos CajasMovimientos)
        {
            if (id != CajasMovimientos.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(CajasMovimientos);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(CajasMovimientos.Id))
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
            return View(CajasMovimientos);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var CajasMovimientos = await this.repository.GetByIdAsync(id);
            if (CajasMovimientos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(CajasMovimientos);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var CajasMovimientos = await repository.GetByIdAsync(id);
            await repository.DeleteAsync(CajasMovimientos);
            return RedirectToAction(nameof(Index));
        }

    }
}
