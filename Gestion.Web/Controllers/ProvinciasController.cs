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
    public class ProvinciasController : Controller
    {
        private readonly IProvinciasRepository repository; 
        private readonly IUserHelper userHelper;

        public ProvinciasController(IProvinciasRepository repository, IUserHelper userHelper)
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

            var Provincias = await this.repository.GetByIdAsync(id);
            if (Provincias == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Provincias);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamProvincias Provincias)
        {
            if (ModelState.IsValid)
            {
                Provincias.Estado = true;
                await repository.CreateAsync(Provincias);
                return RedirectToAction(nameof(Index));
            }
            return View(Provincias);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Provincias = await this.repository.GetByIdAsync(id);
            if (Provincias == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Provincias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamProvincias Provincias)
        {
            if (id != Provincias.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(Provincias);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(Provincias.Id))
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
            return View(Provincias);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Provincias = await this.repository.GetByIdAsync(id);
            if (Provincias == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            Provincias.Estado = !Provincias.Estado;
            await repository.DeleteAsync(Provincias);
            return RedirectToAction(nameof(Index));
        }        

    }
}
