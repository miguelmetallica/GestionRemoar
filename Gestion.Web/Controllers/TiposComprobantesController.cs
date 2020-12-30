using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,TiposComprobantes")]
    public class TiposComprobantesController : Controller
    {
        private readonly ITiposComprobantesRepository repository; 
        private readonly IUserHelper userHelper;

        public TiposComprobantesController(ITiposComprobantesRepository repository, IUserHelper userHelper)
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

            var TiposComprobantes = await this.repository.GetByIdAsync(id);
            if (TiposComprobantes == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(TiposComprobantes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamTiposComprobantes TiposComprobantes)
        {
            if (ModelState.IsValid)
            {
                TiposComprobantes.Estado = true;
                await repository.CreateAsync(TiposComprobantes);
                return RedirectToAction(nameof(Index));
            }
            return View(TiposComprobantes);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var TiposComprobantes = await this.repository.GetByIdAsync(id);
            if (TiposComprobantes == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(TiposComprobantes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamTiposComprobantes TiposComprobantes)
        {
            if (id != TiposComprobantes.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(TiposComprobantes);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(TiposComprobantes.Id))
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
            return View(TiposComprobantes);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var TiposComprobantes = await this.repository.GetByIdAsync(id);
            if (TiposComprobantes == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            TiposComprobantes.Estado = !TiposComprobantes.Estado;
            await repository.DeleteAsync(TiposComprobantes);
            return RedirectToAction(nameof(Index));
        }        

    }
}
