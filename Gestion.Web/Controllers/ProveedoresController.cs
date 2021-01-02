using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,Proveedores")]
    
    public class ProveedoresController : Controller
    {
        private readonly IProveedoresRepository repository; 
        private readonly IUserHelper userHelper;

        public ProveedoresController(IProveedoresRepository repository, IUserHelper userHelper)
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

            var Proveedores = await this.repository.GetByIdAsync(id);
            if (Proveedores == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Proveedores);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proveedores Proveedores)
        {
            if (ModelState.IsValid)
            {
                Proveedores.Estado = true;
                await repository.CreateAsync(Proveedores);
                return RedirectToAction(nameof(Index));
            }
            return View(Proveedores);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Proveedores = await this.repository.GetByIdAsync(id);
            if (Proveedores == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Proveedores);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Proveedores Proveedores)
        {
            if (id != Proveedores.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(Proveedores);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(Proveedores.Id))
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
            return View(Proveedores);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Proveedores = await this.repository.GetByIdAsync(id);
            if (Proveedores == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            //return this.View(Proveedores);
            Proveedores.Estado = !Proveedores.Estado;
            await repository.DeleteAsync(Proveedores);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var Proveedores = await repository.GetByIdAsync(id);
            Proveedores.Estado = !Proveedores.Estado;
            await repository.DeleteAsync(Proveedores);
            return RedirectToAction(nameof(Index));
        }

    }
}
