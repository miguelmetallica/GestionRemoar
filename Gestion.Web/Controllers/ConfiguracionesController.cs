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
    public class ConfiguracionesController : Controller
    {
        private readonly IConfiguracionesRepository repository; 
        private readonly IUserHelper userHelper;

        public ConfiguracionesController(IConfiguracionesRepository repository, IUserHelper userHelper)
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

            var Configuraciones = await this.repository.GetByIdAsync(id);
            if (Configuraciones == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Configuraciones);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SistemaConfiguraciones Configuraciones)
        {
            if (ModelState.IsValid)
            {
                Configuraciones.Estado = true;
                await repository.CreateAsync(Configuraciones);
                return RedirectToAction(nameof(Index));
            }
            return View(Configuraciones);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Configuraciones = await this.repository.GetByIdAsync(id);
            if (Configuraciones == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            return this.View(Configuraciones);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, SistemaConfiguraciones Configuraciones)
        {
            if (id != Configuraciones.Id)
            {
                return new NotFoundViewResult("NoExiste");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    await repository.UpdateAsync(Configuraciones);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await repository.ExistAsync(Configuraciones.Id))
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
            return View(Configuraciones);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            var Configuraciones = await this.repository.GetByIdAsync(id);
            if (Configuraciones == null)
            {
                return new NotFoundViewResult("NoExiste");
            }

            //return this.View(Configuraciones);
            Configuraciones.Estado = !Configuraciones.Estado;
            await repository.DeleteAsync(Configuraciones);
            return RedirectToAction(nameof(Index));
        }      

    }
}
