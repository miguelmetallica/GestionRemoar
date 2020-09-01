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
    public class ComprobantesController : Controller
    {
        private readonly IComprobantesRepository repository; 
        private readonly IUserHelper userHelper;

        public ComprobantesController(IComprobantesRepository repository, IUserHelper userHelper)
        {
            this.repository = repository;
            this.userHelper = userHelper;
        }

        public IActionResult Index()
        {
            return null;
            //return View(repository.sp ());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Comprobantes = await this.repository.GetByIdAsync(id);
            if (Comprobantes == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Comprobantes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comprobantes Comprobantes)
        {
            if (ModelState.IsValid)
            {
                Comprobantes.Estado = true;
                await repository.CreateAsync(Comprobantes);
                return RedirectToAction(nameof(Index));
            }
            return View(Comprobantes);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Comprobantes = await this.repository.GetByIdAsync(id);
            if (Comprobantes == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Comprobantes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Comprobantes Comprobantes)
        {
            if (id != Comprobantes.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(Comprobantes);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(Comprobantes.Id))
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
            return View(Comprobantes);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Comprobantes = await this.repository.GetByIdAsync(id);
            if (Comprobantes == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            Comprobantes.Estado = !Comprobantes.Estado;
            await repository.DeleteAsync(Comprobantes);
            return RedirectToAction(nameof(Index));
        }        

    }
}
