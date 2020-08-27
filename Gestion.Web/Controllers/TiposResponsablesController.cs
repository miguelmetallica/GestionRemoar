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
    public class TiposResponsablesController : Controller
    {
        private readonly ITiposResponsablesRepository repository; 
        private readonly IUserHelper userHelper;

        public TiposResponsablesController(ITiposResponsablesRepository repository, IUserHelper userHelper)
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

            var TiposResponsables = await this.repository.GetByIdAsync(id);
            if (TiposResponsables == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(TiposResponsables);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamTiposResponsables TiposResponsables)
        {
            if (ModelState.IsValid)
            {
                TiposResponsables.Estado = true;
                await repository.CreateAsync(TiposResponsables);
                return RedirectToAction(nameof(Index));
            }
            return View(TiposResponsables);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var TiposResponsables = await this.repository.GetByIdAsync(id);
            if (TiposResponsables == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(TiposResponsables);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamTiposResponsables TiposResponsables)
        {
            if (id != TiposResponsables.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(TiposResponsables);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(TiposResponsables.Id))
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
            return View(TiposResponsables);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var TiposResponsables = await this.repository.GetByIdAsync(id);
            if (TiposResponsables == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            TiposResponsables.Estado = !TiposResponsables.Estado;
            await repository.DeleteAsync(TiposResponsables);
            return RedirectToAction(nameof(Index));
        }        

    }
}
