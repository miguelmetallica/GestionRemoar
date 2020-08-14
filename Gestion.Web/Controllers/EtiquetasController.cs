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
    public class EtiquetasController : Controller
    {
        private readonly IEtiquetasRepository repository; 
        private readonly IUserHelper userHelper;

        public EtiquetasController(IEtiquetasRepository repository, IUserHelper userHelper)
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

            var Etiquetas = await this.repository.GetByIdAsync(id);
            if (Etiquetas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Etiquetas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Etiquetas Etiquetas)
        {
            if (ModelState.IsValid)
            {
                await repository.CreateAsync(Etiquetas);
                return RedirectToAction(nameof(Index));
            }
            return View(Etiquetas);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Etiquetas = await this.repository.GetByIdAsync(id);
            if (Etiquetas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Etiquetas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Etiquetas Etiquetas)
        {
            if (id != Etiquetas.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(Etiquetas);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(Etiquetas.Id))
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
            return View(Etiquetas);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Etiquetas = await this.repository.GetByIdAsync(id);
            if (Etiquetas == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Etiquetas);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var Etiquetas = await repository.GetByIdAsync(id);
            await repository.DeleteAsync(Etiquetas);
            return RedirectToAction(nameof(Index));
        }

    }
}
