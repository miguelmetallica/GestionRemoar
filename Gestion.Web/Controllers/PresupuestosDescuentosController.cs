using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gestion.Web.Controllers
{
    [Authorize(Roles = "Admin,PresupuestosDescuentos")]
    
    public class PresupuestosDescuentosController : Controller
    {
        private readonly IPresupuestosDescuentosRepository repository; 
        private readonly IUserHelper userHelper;

        public PresupuestosDescuentosController(IPresupuestosDescuentosRepository repository, IUserHelper userHelper)
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

            var PresupuestosDescuentos = await this.repository.GetByIdAsync(id);
            if (PresupuestosDescuentos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(PresupuestosDescuentos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParamPresupuestosDescuentos PresupuestosDescuentos)
        {
            if (ModelState.IsValid)
            {
                PresupuestosDescuentos.Estado = true;
                await repository.CreateAsync(PresupuestosDescuentos);
                return RedirectToAction(nameof(Index));
            }
            return View(PresupuestosDescuentos);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var PresupuestosDescuentos = await this.repository.GetByIdAsync(id);
            if (PresupuestosDescuentos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(PresupuestosDescuentos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ParamPresupuestosDescuentos PresupuestosDescuentos)
        {
            if (id != PresupuestosDescuentos.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.UpdateAsync(PresupuestosDescuentos);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(PresupuestosDescuentos.Id))
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
            return View(PresupuestosDescuentos);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var PresupuestosDescuentos = await this.repository.GetByIdAsync(id);
            if (PresupuestosDescuentos == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            PresupuestosDescuentos.Estado = !PresupuestosDescuentos.Estado;
            await repository.DeleteAsync(PresupuestosDescuentos);
            return RedirectToAction(nameof(Index));
        }        

    }
}
