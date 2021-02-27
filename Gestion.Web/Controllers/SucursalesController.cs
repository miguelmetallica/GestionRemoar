using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,Sucursales")]
    public class SucursalesController : Controller
    {
        private readonly ISucursalesRepository repository; 
        private readonly IUserHelper userHelper;

        public SucursalesController(ISucursalesRepository repository, IUserHelper userHelper)
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

            var Sucursales = await this.repository.GetByIdAsync(id);
            if (Sucursales == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Sucursales);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sucursales Sucursales)
        {
            if (ModelState.IsValid)
            {
                Sucursales.Estado = true;
                Sucursales.Codigo = Sucursales.Codigo.ToUpper();
                Sucursales.Nombre = Sucursales.Nombre.ToUpper();
                await repository.CreateAsync(Sucursales);
                return RedirectToAction(nameof(Index));
            }
            return View(Sucursales);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Sucursales = await this.repository.GetByIdAsync(id);
            if (Sucursales == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Sucursales);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Sucursales Sucursales)
        {
            if (id != Sucursales.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Sucursales.Codigo = Sucursales.Codigo.ToUpper();
                    Sucursales.Nombre = Sucursales.Nombre.ToUpper();

                    await repository.UpdateAsync(Sucursales);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(Sucursales.Id))
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
            return View(Sucursales);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Sucursales = await this.repository.GetByIdAsync(id);
            if (Sucursales == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            Sucursales.Estado = !Sucursales.Estado;
            await repository.DeleteAsync(Sucursales);
            return RedirectToAction(nameof(Index));
        }        

    }
}
