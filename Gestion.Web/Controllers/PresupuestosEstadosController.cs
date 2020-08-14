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
    public class PresupuestosEstadosController : Controller
    {
        private readonly IPresupuestosEstadosRepository repository; 
        private readonly IUserHelper userHelper;

        public PresupuestosEstadosController(IPresupuestosEstadosRepository repository, IUserHelper userHelper)
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

            var PresupuestosEstados = await this.repository.GetByIdAsync(id);
            if (PresupuestosEstados == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(PresupuestosEstados);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamPresupuestosEstados PresupuestosEstados)
        {
            if (ModelState.IsValid)
            {
                PresupuestosEstados.Estado = true;
                await repository.CreateAsync(PresupuestosEstados);
                return RedirectToAction(nameof(Index));
            }
            return View(PresupuestosEstados);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var PresupuestosEstados = await this.repository.GetByIdAsync(id);
            if (PresupuestosEstados == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(PresupuestosEstados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamPresupuestosEstados PresupuestosEstados)
        {
            if (id != PresupuestosEstados.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PresupuestosEstados.Estado = true;
                    await repository.UpdateAsync(PresupuestosEstados);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(PresupuestosEstados.Id))
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
            return View(PresupuestosEstados);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var PresupuestosEstados = await this.repository.GetByIdAsync(id);
            if (PresupuestosEstados == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(PresupuestosEstados);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var PresupuestosEstados = await repository.GetByIdAsync(id);
            await repository.DeleteAsync(PresupuestosEstados);
            return RedirectToAction(nameof(Index));
        }

    }
}
